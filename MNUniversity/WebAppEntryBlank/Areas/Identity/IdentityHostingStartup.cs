using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppEntryBlank.Areas.Identity.Data;
using WebAppEntryBlank.Data;

[assembly: HostingStartup(typeof(WebAppEntryBlank.Areas.Identity.IdentityHostingStartup))]
namespace WebAppEntryBlank.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WebAppEntryBlankContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WebAppEntryBlankContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<WebAppEntryBlankContext>();
            });
        }
    }
}