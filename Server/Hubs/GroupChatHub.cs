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
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;

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
			.Where(p => p.Participant.ParticipantType == ChatParticipantTypeEnum.Group);
	}

	public static IEnumerable<ParticipantResponseViewModel> ConnectedParticipants(string currentUserId)
	{
		return FilteredGroupParticipants(currentUserId).Where(x => x.Participant.Email != currentUserId);
	}

	public static IEnumerable<ParticipantResponseViewModel> getConnectedParticpant(string UserId)
	{
		return AllConnectedParticipants.Where(d => d.Participant.UserId == UserId);
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
				Participant = new GroupChatParticipantViewModel()
				{
					DisplayName = userName,
					Email = Context.User.Identity.Name,
					Status = EnumChartParticipantStatus.Online,
					UserId = Context.User.GetClaim(OpenIdConnectConstants.Claims.Subject),
					HubContextId = Context.ConnectionId
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
			Message = message.Message,
			ChatGroup = this._ctx.ChatGroups.FirstOrDefault(d => d.Id.ToString() == message.GroupId)
		};

		var participants = new List<Participant>();
		foreach (var item in usersInGroupToNotify)
		{
			var participant = new Participant();
			participant.User = this._ctx.Users.FirstOrDefault(d => d.Email == item.Participant.DisplayName);
			participants.Add(participant);
		}

		this._ctx.ChatMessages.Add(msg);
		this._ctx.SaveChanges();
	}

	public async Task SendMessage(MessageViewModel message)
	{

		var sender = AllConnectedParticipants.Find(x => x.Participant.HubContextId == message.FromUser.HubContextId 
			&& x.Participant.UserId == Context.User.GetClaim(OpenIdConnectConstants.Claims.Subject));

		if (sender != null)
		{
			var userList = await (from p in this._ctx.Participants
						   where p.Group.Id.ToString() == message.GroupId
						   select p.User.Id).ToArrayAsync();

			// Notify all users in the group except the sender
			var usersInGroupToNotify = AllConnectedParticipants
									   .Where(p => p.Participant.HubContextId != sender.Participant.HubContextId
											  && userList.Contains(p.Participant.UserId) );

			this.insertMessage(usersInGroupToNotify, message, sender);

			Clients.Clients(usersInGroupToNotify.Select(d => d.Participant.HubContextId).ToList()).SendAsync("messageReceived", message);

		}
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		lock (ParticipantsConnectionLock)
		{
			var connectionIndex = AllConnectedParticipants.FindIndex(x => x.Participant.HubContextId == Context.ConnectionId);

			if (connectionIndex >= 0)
			{
				var participant = AllConnectedParticipants.ElementAt(connectionIndex);

				AllConnectedParticipants.Remove(participant);

				Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
			}

			return base.OnDisconnectedAsync(exception);
		}
	}

	public override Task OnConnectedAsync()
	{
		lock (ParticipantsConnectionLock)
		{
			var connectionIndex = DisconnectedParticipants.FindIndex(x => x.Participant.HubContextId == Context.ConnectionId);

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