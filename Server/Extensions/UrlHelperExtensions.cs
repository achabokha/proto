using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Server
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper,  HttpRequest request, string userId, string code)
        {
            return $"{request.Scheme}://{request.Host.ToString()}/confirmemail?userId={userId}&code={WebUtility.UrlEncode(code)}";
        }

        public static string ResetPasswordCallbackLink2(this IUrlHelper urlHelper, HttpRequest request, string code)
        {
            return $"{request.Scheme}://{request.Host.ToString()}/resetpassword?code={WebUtility.UrlEncode(code)}";
            //return $"{request.Scheme}://{request.Host.ToString()}/resetpassword/code={code}";
        }
    }
}
