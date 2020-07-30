using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Models.Instructors;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class InstructorController : ControllerBase
	{
		protected readonly IInstructorService _service;

		public InstructorController(IInstructorService service)
		{
			_service = service;
		}
		[HttpGet]
		public async Task<IEnumerable<InstructorModel>> GetList()
		{
			return await _service.GetList();
		}

		[HttpPut]
		public IActionResult Update(int? id, string[] selectedCourses)
		{
			_service.Update(id, selectedCourses);
			return NoContent();
		}

		[HttpGet("{id}/{courseId}")]
		public async Task<IEnumerable<InstructorModelData>> GetById(int id, int courseId)
		{
			return await _service.GetById(id, courseId);
		}
	}
}
