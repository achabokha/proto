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
using Models.Entities.Chat;

[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
public class GroupChatHub : Hub
{
	private readonly Models.DbContext _ctx;
	private static List<ChatParticipantViewModel> AllConnectedParticipants { get; set; } = new List<ChatParticipantViewModel>();
	private static List<ChatParticipantViewModel> DisconnectedParticipants { get; set; } = new List<ChatParticipantViewModel>();
	private object ParticipantsConnectionLock = new object();


	public GroupChatHub(Models.DbContext ctx)
	{
		this._ctx = ctx;
	}


	public static IEnumerable<ChatParticipantViewModel> ConnectedParticipants(string currentUserId)
	{
		return AllConnectedParticipants;
	}

	public static IEnumerable<ChatParticipantViewModel> getConnectedParticpant(string UserId)
	{
		return AllConnectedParticipants.Where(d => d.UserId == UserId);
	}

	public void Join(string userName)
	{
		lock (ParticipantsConnectionLock)
		{
			var userId = Context.User.GetClaim(OpenIdConnectConstants.Claims.Subject);
			AllConnectedParticipants.Add(new ChatParticipantViewModel()
			{
				DisplayName = userName,
				Email = Context.User.Identity.Name,
				Status = EnumChatParticipantStatus.Online,
				UserId = userId,
				HubContextId = Context.ConnectionId
			});

			var chatGrooup = this._ctx.Participants.Include(d => d.User)
			.Join(
				this._ctx.Participants.Include(d => d.User)
				, p1 => p1.Group.Id, p2 => p2.Group.Id,
				(p1, p2) => new { p2, p1 })
			.Join(
				AllConnectedParticipants,
				p => p.p2.User.Id, cp => cp.UserId,
				(p, cp) => new { p2 = p.p2, p1 = p.p1, HubContextId = cp.HubContextId }
			)
			.Where(d => d.p2.Id != d.p1.Id && d.p1.User.Id == userId)
			.Select(d => d.HubContextId);

			// This will be used as the user's unique ID to be used on ng-chat as the connected user.
			// You should most likely use another ID on your application
			Clients.Caller.SendAsync("generatedUserId", Context.ConnectionId);


			Clients.Clients(chatGrooup.Select(d => d).ToArray()).SendAsync("friendsListChanged", AllConnectedParticipants.Last());


		}
	}

	public void GroupCreated(GroupChatParticipantViewModel group)
	{

	}

	private void insertMessage(IEnumerable<ChatParticipantViewModel> usersInGroupToNotify, MessageViewModel message, ChatParticipantViewModel sender)
	{
		var fromUser = this._ctx.Users.FirstOrDefault(d => d.Id == sender.UserId);
		var m = new ChatMessageSeen()
		{
			DateSeen = DateTime.UtcNow,
			User = fromUser
		};
		var msg = new ChatMessage()
		{
			DownloadUrl = message.DownloadUrl,
			DateSent = message.DateSent,
			FileSizeInBytes = message.FileSizeInBytes,
			FromUser = fromUser,
			Message = message.Message,
			ChatGroup = this._ctx.ChatGroups.FirstOrDefault(d => d.Id.ToString() == message.GroupId)
		};

		var participants = new List<Participant>();
		foreach (var item in usersInGroupToNotify)
		{
			var participant = new Participant();
			participant.User = this._ctx.Users.FirstOrDefault(d => d.Id == item.UserId);
			participants.Add(participant);
		}

		this._ctx.ChatMessages.Add(msg);
		this._ctx.SaveChanges();

	}

	public async Task ChatMessageSeen(ChatMessageSeenViewModel message)
	{
		var userId = Context.User.GetClaim(OpenIdConnectConstants.Claims.Subject);


		var msg = await this._ctx.ChatGroups
			.Include(d => d.DateSeen)
			.ThenInclude(s => s.User)
			.FirstOrDefaultAsync(d => d.Id.ToString() == message.groupId );
		var chatMsgSeen = msg.DateSeen.FirstOrDefault(d => d.User.Id == userId);
		if (chatMsgSeen == null)
		{
			msg.DateSeen.Add(new ChatMessageSeen() {
				DateSeen = DateTime.UtcNow,
				User = this._ctx.Users.FirstOrDefault(d => d.Id == userId)
			});
		}
		else
		{
			chatMsgSeen.DateSeen = DateTime.UtcNow;
			chatMsgSeen.User = await this._ctx.Users.FirstOrDefaultAsync(d => d.Id == userId);
		}

		await this._ctx.SaveChangesAsync();
	}
	public async Task SendMessage(MessageViewModel message)
	{

		var sender = AllConnectedParticipants.Find(x => x.HubContextId == message.FromUser.HubContextId
			&& x.UserId == Context.User.GetClaim(OpenIdConnectConstants.Claims.Subject));

		if (sender != null)
		{
			var userList = await (from p in this._ctx.Participants
								  where p.Group.Id.ToString() == message.GroupId
								  select p.User.Id).ToArrayAsync();

			// Notify all users in the group except the sender
			var usersInGroupToNotify = AllConnectedParticipants
									   .Where(p => p.HubContextId != sender.HubContextId
											  && userList.Contains(p.UserId));

			this.insertMessage(usersInGroupToNotify, message, sender);

			Clients.Clients(usersInGroupToNotify.Select(d => d.HubContextId).ToList()).SendAsync("messageReceived", message);

		}
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		lock (ParticipantsConnectionLock)
		{
			var connectionIndex = AllConnectedParticipants.FindIndex(x => x.HubContextId == Context.ConnectionId);

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
			var connectionIndex = DisconnectedParticipants.FindIndex(x => x.HubContextId == Context.ConnectionId);

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