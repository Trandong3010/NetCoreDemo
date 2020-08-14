using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
	public abstract class ApplicationRole : IdentityRole
	{
		public string Description { get; set; }
		public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
		public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
	}

	public abstract class ApplicationRoleClaim : IdentityRoleClaim<string>
	{
		public virtual ApplicationRole Role { get; set; }
	}
}
