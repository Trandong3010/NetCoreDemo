using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entity;
using Infrastructure.Models.Enrollment;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class EnrollmentController : ControllerBase
	{
		protected readonly IEnrollmentService _service;

		public EnrollmentController(IEnrollmentService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IEnumerable<EnrollmentModel>> GetList()
		{
			return await _service.GetList();
		}

		[HttpPost("id")]
		public IActionResult Create(EnrollmentModel model)
		{
			_service.Insert(model);
			return NoContent();
		}

		[HttpPut]
		public IActionResult Update(EnrollmentModel model)
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
		public EnrollmentModel GetById(int id)
		{
			return _service.GetById(id);
		}
	}
}
