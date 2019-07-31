using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Embily.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers.Api.v2
{
    public class ApiBaseController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ApiBaseController(IProgramService programService)
        {
            _programService = programService;
        }

        protected string GetProgramId()
        {
            return _programService.GetProgramByUserId(this.GetUserId()).ProgramId;
        }

        protected string GetUserId()
        {
            return User.GetClaim(OpenIdConnectConstants.Claims.Subject);
        }
    }
}
