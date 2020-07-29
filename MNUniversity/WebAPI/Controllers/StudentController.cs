using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entity;
using Infrastructure.Models.Students;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly IStudentService _service;

		public StudentController(IStudentService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IEnumerable<StudentModel>> GetList()
		{ 
			return await _service.GetList();
		}

		[HttpPost("id")]
		public IActionResult Create(StudentModel model)
		{
			_service.Insert(model);
			return NoContent();
		}

		[HttpPut]
		public IActionResult Update(StudentModel model)
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
