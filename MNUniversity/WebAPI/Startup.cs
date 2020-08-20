using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using DataAccess.Context;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Common.HttpResponseException;
using Infrastructure.Models;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<SchoolContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"), b => b.MigrationsAssembly("DataAccess")));
			services.AddTransient<IStudentService, StudentService>();
			services.AddTransient<ICourseService, CourseService>();
			services.AddTransient<IEnrollmentService, EnrollmentService>();
			services.AddTransient<IDepartmentService, DepartmentService>();
			services.AddTransient<IInstructorService, InstructorService>();
			services.AddScoped<IUserService, UserService>();

			services.AddSwaggerGen(c => c.SwaggerDoc("v1",
				new OpenApiInfo {Title = "MNUniversity", Version = "v1", Description = "API to University"}));

			services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

			services.AddControllers().ConfigureApiBehaviorOptions(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var result = new BadRequestObjectResult(context.ModelState);

					result.ContentTypes.Add(MediaTypeNames.Application.Json);
					result.ContentTypes.Add(MediaTypeNames.Application.Xml);

					return result;
				};
				options.SuppressConsumesConstraintForFormFileParameters = true;
				options.SuppressInferBindingSourcesForParameters = true;
				options.SuppressMapClientErrors = true;
				options.SuppressModelStateInvalidFilter = true;
				options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
			});
			services.AddProblemDetails();
			services.AddControllers().AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			);
			services.AddAutoMapper(typeof(AutoMapping));

			services.AddCors();
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// ===== Add Jwt Authentication ========
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

				})
				.AddJwtBearer(cfg =>
				{
					cfg.RequireHttpsMetadata = false;
					cfg.SaveToken = true;
					cfg.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = Configuration["JwtIssuer"],
						ValidAudience = Configuration["JwtIssuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
						ClockSkew = TimeSpan.Zero // remove delay of token when expire
					};
				});

			// Add Identity
			services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<SchoolContext>();

			services.Configure<IdentityOptions>(options =>
			{
				// Defaul Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// Default Password settings
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				// Default SignIn Setting.
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;

				// Default User settings.
				options.User.AllowedUserNameCharacters =
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = false;
			});

			services.Configure<PasswordHasherOptions>(options => { options.IterationCount = 12000; });

			// IIS/IIS Express
			services.AddAuthentication(IISDefaults.AuthenticationScheme);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SchoolContext context)
		{
			app.UseProblemDetails();
			// Enable middleware to serve generated Swagger as a JSON endpoint
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
			// specifying the Swagger JSON endpoint
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MNUniversity");
			});

			app.UseExceptionHandler(env.IsDevelopment() ? "/error-local-development" : "/error");

			// global core policy
			app.UseCors(x => x.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
