using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Collections.Generic;
using Server.Hubs.Models;
using System;

namespace Server.Controllers.Hubs
{
	[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
	[Route("[controller]")]
	public class HubsController : BaseController
	{
		private object ChatGroupCreationLock = new object();
		readonly Models.DbContext _ctx;


		public HubsController(Models.DbContext ctx)
		{
			_ctx = ctx;
		}

		// Sending the userId from the request body as this is just a demo. 
		// On your application you probably want to fetch this from your authentication context and not receive it as a parameter
		[HttpPost("[action]")]
		public IActionResult ListFriends([FromBody] dynamic payload)
		{
			string participantUserId = this.HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject);
			var lstChats = (from g in this._ctx.ChatGroups
							.Include(d => d.Participants)
							where g.Participants.Any(d => d.User.Id == participantUserId)
							select new { g.Participants });

			return Json(lstChats);

			// Use the following for group chats
			// Make sure you have [pollFriendsList] set to true for this simple group chat example to work as
			// broadcasting with group was not implemented here
			// return Json(GroupChatHub.ConnectedParticipants((string)payload.currentUserId));
		}

		[HttpPost("[action]")]
		public async Task<JsonResult> CreateGroup([FromBody] dynamic payload)
		{
			List<string> userList = new List<string>();
			foreach (var item in payload.group.chattingTo)
			{
				userList.Add((string)item.userId);
			};
			
			string participantUserId = this.HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject);
			if(!userList.Contains(participantUserId)) {
				userList.Add(participantUserId);
			}

			var groupId = (string)payload.group.groupId;
			var chatGroup = new ChatGroup();
			if (string.IsNullOrEmpty(groupId))
			{
				chatGroup = await (from gr in this._ctx.ChatGroups
								   join p in this._ctx.Participants.Include(d => d.User) on gr equals p.Group into arrPart
								   where
									   arrPart.Count() == userList.Count
									&& gr.ParticipantType == EnumChatGroupParticipantType.user
								   	&& arrPart.All(d => userList.Contains(d.User.Id))
								   select gr
				).FirstOrDefaultAsync();
			}
			else
			{
				chatGroup = await this._ctx.ChatGroups.FirstOrDefaultAsync(d => d.Id.ToString() == groupId);
			}
			lock (ChatGroupCreationLock)
			{
				if (chatGroup == null)
				{
					chatGroup = new ChatGroup();
					var participants = new List<Participant>();
					foreach (var item in userList)
					{
						var participant = new Participant();
						participant.User = this._ctx.Users.Find(item);
						participants.Add(participant);
					}
					if (payload.group.participantType == 1)
					{
						var participant = new Participant();
						participant.User = this._ctx.Users.Find(HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject));
						participants.Add(participant);
					}

					chatGroup.Participants = participants;
					chatGroup.DateCreated = DateTime.UtcNow;
					chatGroup.Title = "Some tittle " + DateTime.Now.ToShortDateString();
					this._ctx.ChatGroups.Add(chatGroup);
					this._ctx.SaveChanges();
				}
			}

			return Json(new { chatRoomId = chatGroup.Id });
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> UserList([FromBody] dynamic payload)
		{
			string srchTxt = payload.searchText;
			string userId = this.HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject);
			var connectedPart = GroupChatHub.getConnectedParticpant(userId).FirstOrDefault();

			var userList = (
				from u in this._ctx.Users
				where u.Id != userId
				orderby u.Email
				select new
				{
					DisplayName = u.Email,
					UserId = u.Id,
					ParticipantType = ChatParticipantTypeEnum.User,
					Status = connectedPart == null ? EnumChatParticipantStatus.Offline : EnumChatParticipantStatus.Online,
					Email = u.Email
				}
			);


			return Json(await userList.ToArrayAsync());
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Participants([FromBody] dynamic payload)
		{
			string srchTxt = payload.searchText;


			string userId = this.HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject);
			string userEmail = this._ctx.Users.Find(userId).Email;
			var msgList = await (from g in this._ctx.ChatGroups
								 join p in this._ctx.Participants.Include(d => d.User) on g equals p.Group into arrPart
								 where g.ParticipantType == EnumChatGroupParticipantType.user
								 && arrPart.Any(d => d.User.Id == userId)

								 select new { Particpants = arrPart, ChatGroup = g }
						   ).ToArrayAsync();
			var userList = await (
				from u in this._ctx.Users
				where !msgList.Any(d => d.Particpants.Any(p => p.User == u))
				&& u.Id != userId
				select new { Id = u.Id, Email = u.Email, }
			).ToArrayAsync();


			List<ParticipantResponseViewModel> resp = new List<ParticipantResponseViewModel>();

			foreach (var item in msgList)
			{
				var part = new ParticipantResponseViewModel();
				part.Participants = new List<ChatParticipantViewModel>();


				foreach (var p in item.Particpants.OrderBy(d => d.User.Id == userId))
				{
					var connectedPart = GroupChatHub.getConnectedParticpant(p.User.Id).FirstOrDefault();

					part.Participants.Add(new ChatParticipantViewModel()
					{

						DisplayName = p.User.Email,
						UserId = p.User.Id,
						ParticipantType = ChatParticipantTypeEnum.User,
						Status = connectedPart == null ? EnumChatParticipantStatus.Offline : EnumChatParticipantStatus.Online,
						Email = p.User.Email
					});
				}



				part.Metadata = new ParticipantMetadataViewModel()
				{
					TotalUnreadMessages = 0,
					Title = item.ChatGroup.Title,
					GroupId = item.ChatGroup != null ? item.ChatGroup.Id.ToString() : ""
				};
				resp.Add(part);
			}


			foreach (var user in userList)
			{
				var part = new ParticipantResponseViewModel();
				part.Participants = new List<ChatParticipantViewModel>();



				var connectedPart = GroupChatHub.getConnectedParticpant(user.Id).FirstOrDefault();

				part.Participants.Add(new ChatParticipantViewModel()
				{

					DisplayName = user.Email,
					UserId = user.Id,
					ParticipantType = ChatParticipantTypeEnum.User,
					Status = connectedPart == null ? EnumChatParticipantStatus.Offline : EnumChatParticipantStatus.Online,
					Email = user.Email
				});

				part.Participants.Add(new ChatParticipantViewModel()
				{
					DisplayName = userEmail,
					UserId = userId,
					ParticipantType = ChatParticipantTypeEnum.User,
					Status = EnumChatParticipantStatus.Online,
					Email = userEmail
				});



				part.Metadata = new ParticipantMetadataViewModel()
				{
					TotalUnreadMessages = 0,
					GroupId = "",
					Title = part.Participants.First().DisplayName
				};

				resp.Add(part);
			}

			return Json(resp.OrderBy(d => d.Participants.Any(p => p.Status == EnumChatParticipantStatus.Online)).ThenBy(d => d.Metadata.Title));
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> MessageHistory([FromBody] dynamic payload)
		{
			string mailA = payload.mailA;
			string b = payload.mailB;
			string[] mailB = b.Split(",");
			var msgList = await (from m in this._ctx.ChatMessages
							.Include(d => d.FromUser)
							.Include(d => d.ChatGroup.Participants)
								 where (m.FromUser.Email == mailA ||
									 mailB.Contains(m.FromUser.Email)) &&
									 (m.ChatGroup.Participants.Any(p => p.User.Email == mailA
									 || mailB.Contains(m.FromUser.Email)))
								 orderby m.DateSent
								 select m).ToListAsync();

			return Json(msgList);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> GroupMessageHistory([FromBody] dynamic payload)
		{
			string groupId = (string)payload.groupId;
			var msgList = await (from m in this._ctx.ChatMessages
							.Include(d => d.FromUser)
							.Include(d => d.ChatGroup)
							.Include(d => d.DateSeen)
								.ThenInclude(s => s.User)
								 where m.ChatGroup.Id.ToString() == groupId
								 orderby m.DateSent
								 select new
								 {
									 Type = m.Type,
									 GroupId = m.ChatGroup.Id,
									 Message = m.Message,
									 DateSent = m.DateSent,
									 DateSeen = m.DateSeen.Select(d => new { UserId = d.User.Id, DateSeen = d.DateSeen, MsgId = m.Id }),
									 Id = m.Id,
									 FromUser = new
									 {
										 UserId = m.FromUser.Id,
										 Status = EnumChatParticipantStatus.Online,
										 ParticipantType = EnumChatGroupParticipantType.user,
										 Avatar = "",
										 displayName = m.FromUser.UserName
									 }
								 }).ToListAsync();

			return Json(msgList);
		}
	}
}