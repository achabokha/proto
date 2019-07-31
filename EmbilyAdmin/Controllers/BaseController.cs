using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace EmbilyAdmin.Controllers
{
    public class BaseController : Controller
    {
        // only for Authenticated API
        protected string GetUserId()
        {
            return User.GetClaim(OpenIdConnectConstants.Claims.Subject);
        }
    }
}