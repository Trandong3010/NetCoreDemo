using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entity;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase
	{
		protected readonly IDepartmentService _service;

		public DepartmentController(IDepartmentService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IEnumerable<Department>> GetList()
		{
			return await _service.GetList();
		}
	}
}
