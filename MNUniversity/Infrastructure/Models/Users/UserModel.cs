using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infrastructure.Models.Users
{
	public class UserModel
	{
		public string Id { get; set; }
		[StringLength(256)]
		public string UserName { get; set; }
		[StringLength(256)]
		public string NormalizedUserName { get; set; }
		[StringLength(256)]
		public string Email { get; set; }
		[StringLength(256)]
		public string NormalizedEmail { get; set; }
		public bool EmailConfirmed { get; set; }
		[MaxLength]
		public string PasswordHash { get; set; }
		[MaxLength]
		public string SecurityStamp { get; set; }
		[MaxLength]
		public string ConcurrencyStamp { get; set; }
		[MaxLength]
		public string PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public bool TwoFactorEnabled { get; set; }
		public DateTimeOffset? LockoutEnd { get; set; }
		public bool LockoutEnabled { get; set; }
		public int AccessFailedCount { get; set; }
		[MaxLength]
		public string Name { get; set; }
		public DateTime Dob { get; set; }
    }
}
