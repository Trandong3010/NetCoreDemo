using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
	public abstract class ApplicationUser : IdentityUser
	{
		public string CustomTag { get; set; }

		public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
		public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
		public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
		public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
	}

	public abstract class ApplicationUserRole : IdentityUserRole<string>
	{
		public virtual ApplicationUser User { get; set; }
		public virtual ApplicationRole Role { get; set; }
	}

	public abstract class ApplicationUserClaim : IdentityUserClaim<string>
	{
		public virtual ApplicationUser User { get; set; }
	}

	public abstract class ApplicationUserLogin : IdentityUserLogin<string>
	{
		public virtual ApplicationUser User { get; set; }
	}

	public abstract class ApplicationUserToken : IdentityUserToken<string>
	{
		public virtual ApplicationUser User { get; set; }
	}
}
