using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entity;
using Infrastructure.Models.Courses;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CourseController : ControllerBase
	{
		protected readonly ICourseService _service;

		public CourseController(ICourseService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IEnumerable<CourseModel>> GetList()
		{
			return await _service.GetList();
		}
		[HttpPost]
		public IActionResult Create(CourseModel model)
		{
			 _service.Insert(model); 
			 return NoContent();
		}

		[HttpPut]
		public IActionResult Update(CourseModel model)
		{
			_service.Update(model);
			return NoContent();

		}
		[HttpDelete("id")]
		public IActionResult Delete(int id)
		{
			_service.Delete(id);
			return NoContent();
		}
	}
}
