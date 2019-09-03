using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Hubs
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
	[Route("[controller]")]
    public class HubsController: BaseController
    {
        // Sending the userId from the request body as this is just a demo. 
        // On your application you probably want to fetch this from your authentication context and not receive it as a parameter
        [HttpPost("[action]")]
        public IActionResult ListFriends([FromBody] dynamic payload)
        {
            return Json(GroupChatHub.ConnectedParticipants((string)payload.currentUserId));

            // Use the following for group chats
            // Make sure you have [pollFriendsList] set to true for this simple group chat example to work as
            // broadcasting with group was not implemented here
            // return Json(GroupChatHub.ConnectedParticipants((string)payload.currentUserId));
        }
    }
}