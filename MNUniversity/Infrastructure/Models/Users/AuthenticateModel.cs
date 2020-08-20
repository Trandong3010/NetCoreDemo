using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Models.Users
{
	public class AuthenticateModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required, DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
