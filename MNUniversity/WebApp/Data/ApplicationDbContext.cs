using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// define name table
			builder.Entity<IdentityUser>(x => x.ToTable("MyUsers"));
			builder.Entity<IdentityUserClaim<string>>(x => x.ToTable("MyUserClaims"));
			builder.Entity<IdentityUserLogin<string>>(x => x.ToTable("MyUserLogins"));
			builder.Entity<IdentityUserToken<string>>(x => x.ToTable("MyUserTokens"));
			builder.Entity<IdentityRole>(x => x.ToTable("MyRoles"));
			builder.Entity<IdentityRoleClaim<string>>(x => x.ToTable("MyRoleClaims"));
			builder.Entity<IdentityUserRole<string>>(x => x.ToTable("MyUserRoles"));

			builder.Entity<IdentityUser>(x =>
			{
				x.Property(e => e.Email).HasColumnName("EMail");
				x.Property(e => e.UserName).HasMaxLength(128);
				x.Property(e => e.NormalizedUserName).HasMaxLength(128);
				x.Property(e => e.Email).HasMaxLength(128);
				x.Property(e => e.NormalizedEmail).HasMaxLength(128);
			});

			builder.Entity<IdentityUserToken<string>>(x =>
			{
				x.Property(t => t.LoginProvider).HasMaxLength(128);
				x.Property(t => t.Name).HasMaxLength(128);
			});

			builder.Entity<IdentityUserClaim<string>>(x =>
			{
				x.Property(e => e.ClaimType).HasColumnName("CType");
				x.Property(e => e.ClaimValue).HasColumnName("CValue");

			});

			builder.Entity<ApplicationUser>(x =>
			{
				// Each User can have many UserClaims
				x.HasMany(e => e.Claims)
					.WithOne()
					.HasForeignKey(uc => uc.UserId)
					.IsRequired();

				// Each User can have many UserLogins
				x.HasMany(e => e.Logins)
					.WithOne()
					.HasForeignKey(ul => ul.UserId)
					.IsRequired();

				// Each User can have many UserTokens
				x.HasMany(e => e.Tokens)
					.WithOne()
					.HasForeignKey(ul => ul.UserId)
					.IsRequired();

				// Each User can have many entries in the UserRole join table
				x.HasMany(e => e.UserRoles)
					.WithOne(e => e.User)
					.HasForeignKey(ul => ul.UserId)
					.IsRequired();
			});

			builder.Entity<ApplicationRole>(x =>
			{
				// Each Role can have many entries in the UserRole join table
				x.HasMany(e => e.UserRoles)
					.WithOne(e => e.Role)
					.HasForeignKey(ul => ul.RoleId)
					.IsRequired();

				// Each Role can have many associated RoleClaims
				x.HasMany(e => e.RoleClaims)
					.WithOne(e => e.Role)
					.HasForeignKey(ul => ul.RoleId)
					.IsRequired();
			});

			builder.HasDefaultSchema("notdbo");
		}
	}
}
