using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAppEntryBlank.Areas.Identity.Data
{
	public class ApplicationUser : IdentityUser<string>
	{
		public bool IsAdmin { get; set; }

		public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
		public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
		public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
		public virtual ICollection<IdentityRole<string>> UserRoles { get; set; }
	}
}
