using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Automapper;
using Infrastructure.Common.HttpResponseException;
using Infrastructure.Models;
using Infrastructure.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddDbContext<SchoolContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
			services.AddTransient<IStudentService, StudentService>();
			services.AddTransient<ICourseService, CourseService>();
			services.AddTransient<IEnrollmentService, EnrollmentService>();
			services.AddTransient<IDepartmentService, DepartmentService>();
			services.AddTransient<IInstructorService, InstructorService>();

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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
