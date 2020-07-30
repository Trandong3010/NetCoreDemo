using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models.Courses;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

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

		[HttpGet("id")]
		public CourseModel GetById(int id)
		{
			return _service.GetById(id);
		}
	}
}
