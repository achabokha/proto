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
		public JsonResult CreateGroup([FromBody] dynamic payload)
		{
			List<string> group = new List<string>();
			foreach (var item in payload.group.chattingTo)
			{
				group.Add((string)item.userId);
			};
			var groupId = (string)payload.group.groupId;
			var chatGroup = this._ctx.ChatGroups.Find(groupId);
			lock (ChatGroupCreationLock)
			{
				if (chatGroup == null)
				{
					chatGroup = new ChatGroup();
					var participants = new List<Participant>();
					foreach (var item in group)
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
			var msgList = this._ctx.Users.Where(d => d.Id != userId && d.Email.Contains(srchTxt));
			var user = this._ctx.Users.Find(HttpContext.User.GetClaim(OpenIdConnectConstants.Claims.Subject));


			List<ParticipantResponseViewModel> resp = new List<ParticipantResponseViewModel>();

			foreach (var item in msgList)
			{
				var connectedPart = GroupChatHub.getConnectedParticpant(item.Id).FirstOrDefault();

				var part = new ParticipantResponseViewModel();
				var group = new GroupChatParticipantViewModel();
				group.ChattingTo = new List<ChatParticipantViewModel>();

				var chatGroup = await this._ctx.ChatGroups.FirstOrDefaultAsync(d =>
					d.Participants.Any(p => p.User.Id == item.Id)
					&& d.Participants.Any(p => p.User.Id == user.Id)
					&& d.ParticipantType == EnumChatGroupParticipantType.user
				);

				group.ChattingTo.Add(new ChatParticipantViewModel()
				{
					DisplayName = item.Email,
					UserId = item.Id,
					ParticipantType = ChatParticipantTypeEnum.User,
					Status = connectedPart == null ? EnumChartParticipantStatus.Offline : EnumChartParticipantStatus.Online,
					Email = item.Email
				});

				var participant = new ChatParticipantViewModel()
				{
					DisplayName = user.Email,
					UserId = user.Id,
					ParticipantType = ChatParticipantTypeEnum.User,
					Status = EnumChartParticipantStatus.Online,
					Email = user.Email
				};

				group.ChattingTo.Add(participant);

				part.Metadata = new ParticipantMetadataViewModel()
				{
					TotalUnreadMessages = 0,
					GroupId = chatGroup != null ? chatGroup.Id.ToString() : ""
				};
				part.Participant = group;
				if (group.ChattingTo.Count == 2)
				{
					group.Status = group.ChattingTo.First().Status;
				}

				resp.Add(part);
			}



			return Json(resp.OrderBy(d => d.Participant.Status).ThenBy(d => d.Participant.Email));
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
								 where m.ChatGroup.Id.ToString() == groupId
								 orderby m.DateSent
								 select new
								 {
									 Type = m.Type,
									 GroupId = m.ChatGroup.Id,
									 Message = m.Message,
									 DateSent = m.DateSent,
									 DateSeen = m.DateSeen,
									 FromUser = new
									 {
										 UserId = m.FromUser.Id,
										 Status = EnumChartParticipantStatus.Online,
										 ParticipantType = EnumChatGroupParticipantType.user,
										 Avatar = "",
										 displayName = m.FromUser.UserName
									 }
								 }).ToListAsync();

			return Json(msgList);
		}
	}
}