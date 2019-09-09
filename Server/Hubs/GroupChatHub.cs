using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.Entities;
using Server.Hubs.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
public class GroupChatHub : Hub
{
	private readonly Models.DbContext _ctx;
	private static List<ParticipantResponseViewModel> AllConnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
	private static List<ParticipantResponseViewModel> DisconnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
	private static List<GroupChatParticipantViewModel> AllGroupParticipants { get; set; } = new List<GroupChatParticipantViewModel>();
	private object ParticipantsConnectionLock = new object();

	public GroupChatHub(Models.DbContext ctx)
	{
		this._ctx = ctx;
	}

	private static IEnumerable<ParticipantResponseViewModel> FilteredGroupParticipants(string currentUserId)
	{
		return AllConnectedParticipants
			.Where(p => p.Participant.ParticipantType == ChatParticipantTypeEnum.User
				   || AllGroupParticipants.Any(g => g.Id == p.Participant.Id && g.ChattingTo.Any(u => u.Id == currentUserId))
			);
	}

	public static IEnumerable<ParticipantResponseViewModel> ConnectedParticipants(string currentUserId)
	{
		return FilteredGroupParticipants(currentUserId).Where(x => x.Participant.Id != currentUserId);
	}

	public void Join(string userName)
	{
		lock (ParticipantsConnectionLock)
		{
			AllConnectedParticipants.Add(new ParticipantResponseViewModel()
			{
				Metadata = new ParticipantMetadataViewModel()
				{
					TotalUnreadMessages = 0
				},
				Participant = new ChatParticipantViewModel()
				{
					DisplayName = userName,
					Id = Context.ConnectionId
				}
			});

			// This will be used as the user's unique ID to be used on ng-chat as the connected user.
			// You should most likely use another ID on your application
			Clients.Caller.SendAsync("generatedUserId", Context.ConnectionId);

			Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
		}
	}

	public void GroupCreated(GroupChatParticipantViewModel group)
	{
		AllGroupParticipants.Add(group);

		// Pushing the current user to the "chatting to" list to keep track of who's created the group as well.
		// In your application you'll probably want a more sofisticated group persistency and management
		group.ChattingTo.Add(new ChatParticipantViewModel()
		{
			Id = Context.ConnectionId
		});

		AllConnectedParticipants.Add(new ParticipantResponseViewModel()
		{
			Metadata = new ParticipantMetadataViewModel()
			{
				TotalUnreadMessages = 0
			},
			Participant = group
		});

		Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
	}

	private void insertMessage(IEnumerable<ParticipantResponseViewModel> usersInGroupToNotify, MessageViewModel message, ParticipantResponseViewModel sender)
	{
		var msg = new ChatMessage()
		{
			DateSeen = message.DateSeen,
			DownloadUrl = message.DownloadUrl,
			DateSent = message.DateSent,
			FileSizeInBytes = message.FileSizeInBytes,
			FromUser = this._ctx.Users.FirstOrDefault(d => d.Email == sender.Participant.DisplayName),
			Message = message.Message
		};

		var participants = new List<Participant>();
		foreach (var item in usersInGroupToNotify)
		{
			var participant = new Participant();
			participant.User = this._ctx.Users.FirstOrDefault(d => d.Email == item.Participant.DisplayName);
			participants.Add(participant);
		}

		var group = this._ctx.ChatGroups.Include(d => d.Participants).FirstOrDefault(d =>
			d.Participants.All(dp => participants.Exists(p => p.User.Email == dp.User.Email))
		);


		msg.ChatGroup = group;

		this._ctx.ChatMessages.Add(msg);
		this._ctx.SaveChanges();
	}

	public void SendMessage(MessageViewModel message)
	{
		var sender = AllConnectedParticipants.Find(x => x.Participant.Id == message.FromId);

		if (sender != null)
		{
			var groupDestinatary = AllGroupParticipants.Where(x => x.Id == message.ToId).FirstOrDefault();

			if (groupDestinatary != null)
			{
				// Notify all users in the group except the sender
				var usersInGroupToNotify = AllConnectedParticipants
										   .Where(p => p.Participant.Id != sender.Participant.Id
												  && groupDestinatary.ChattingTo.Any(g => g.Id == p.Participant.Id)
										   );

                this.insertMessage(usersInGroupToNotify, message, sender);

				Clients.Clients(usersInGroupToNotify.Select(d => d.Participant.Id).ToList()).SendAsync("messageReceived", groupDestinatary, message);
			}
			else
			{
                var usersInGroupToNotify = AllConnectedParticipants
										   .Where(p => p.Participant.Id != sender.Participant.Id
												  && p.Participant.Id == sender.Participant.Id
										   );
				this.insertMessage(usersInGroupToNotify, message, sender);
				Clients.Client(message.ToId).SendAsync("messageReceived", sender.Participant, message);
			}
		}
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		lock (ParticipantsConnectionLock)
		{
			var connectionIndex = AllConnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

			if (connectionIndex >= 0)
			{
				var participant = AllConnectedParticipants.ElementAt(connectionIndex);

				var groupsParticipantIsIn = AllGroupParticipants.Where(x => x.ChattingTo.Any(u => u.Id == participant.Participant.Id));

				AllConnectedParticipants.RemoveAll(x => groupsParticipantIsIn.Any(g => g.Id == x.Participant.Id));
				AllGroupParticipants.RemoveAll(x => groupsParticipantIsIn.Any(g => g.Id == x.Id));

				AllConnectedParticipants.Remove(participant);
				DisconnectedParticipants.Add(participant);

				Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
			}

			return base.OnDisconnectedAsync(exception);
		}
	}

	public override Task OnConnectedAsync()
	{
		lock (ParticipantsConnectionLock)
		{
			var connectionIndex = DisconnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

			if (connectionIndex >= 0)
			{
				var participant = DisconnectedParticipants.ElementAt(connectionIndex);

				DisconnectedParticipants.Remove(participant);
				AllConnectedParticipants.Add(participant);

				Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
			}

			return base.OnConnectedAsync();
		}
	}
}