using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Server.ViewModels
{
	public class UserRegisterViewModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public string LastName { get; set; }

		public string FirstName { get; set; }

		public string UserName { get; set; }

		public string Token { get; set; }
	}

	public class ConfirmEmail
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string Code { get; set; }
	}
}