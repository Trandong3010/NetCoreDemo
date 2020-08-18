using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Common.Authentication;
using Infrastructure.Models.Users;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;

		public UserController(IUserService service)
		{
			_service = service;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public IActionResult Register([FromBody]RegisterModel model)
		{
			try
			{
				_service.Register(model);
				return Ok();
			}
			catch (AppException ex)
			{
				// return error message if there was an exception
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IEnumerable<UserModel>> GetList()
		{
			return await _service.GetAll();
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody] AuthenticateModel model)
		{
			var user = _service.Authenticate(model.Username, model.Password);

			if (user == null) return BadRequest(new {message = "Username or password is incorrect"});
			
			// var tokenHandler = new JwtSecurityTokenHandler();
			
			
			return null;
		}
	}
}
