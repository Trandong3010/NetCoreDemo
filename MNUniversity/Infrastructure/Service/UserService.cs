using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service
{
	public interface IUserService
	{
		Task<object> Authenticate(string username, string password);
		Task<object> GetAll();
		Task<object> Register([FromBody]RegisterModel model);
	}

	public class UserService : IUserService
	{
		private readonly IMapper _mapper;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration;


		public UserService(IMapper mapper, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			_mapper = mapper;
			_signInManager = signInManager;
			_userManager = userManager;
			this._configuration = configuration;
		}

		public async Task<object> Authenticate(string Email, string Password)
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) return null;

			var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);

			if (result.Succeeded)
			{
				var appUser = _userManager.Users.SingleOrDefault(r => r.Email == Email);
				return await GenerateJwtToken(Email, appUser);
			}

			throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
		}

		private async Task<object> GenerateJwtToken(string email, IdentityUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

			var token = new JwtSecurityToken(
				_configuration["JwtIssuer"],
				_configuration["JwtIssuer"],
				claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<object> GetAll()
		{
			return await _userManager.Users.ToListAsync();
		}

		public async Task<object> Register([FromBody]RegisterModel model)
		{
			var user = new IdentityUser
			{
				UserName = model.Email,
				Email = model.Email
			};
			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				return await GenerateJwtToken(model.Email, user);
			}

			throw new ApplicationException("UNKNOWN_ERROR");
		}
	}
}
