using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Models.Users
{
	public class AuthenticateModel
	{
		[Required, MaxLength(256)]
		public string UserName { get; set; }
		[Required, DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
