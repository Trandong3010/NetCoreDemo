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
using Infrastructure.Common.Authentication;
using Infrastructure.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service
{
	public interface IUserService
	{
		AuthenticateResponse Authenticate(string username, string password);
		Task<IEnumerable<UserModel>> GetAll();
		AspNetUsers GetById(string id);
		AspNetUsers Create(AspNetUsers user, string password);
		void Register([FromBody]RegisterModel model);
	}

	public class UserService : IUserService
	{
		protected readonly SchoolContext _context;
		private readonly IMapper _mapper;
		private readonly AppSettings _appSettings;

		public UserService(SchoolContext context, IMapper mapper, IOptions<AppSettings> appSettings)
		{
			_context = context;
			_mapper = mapper;
			_appSettings = appSettings.Value;
		}

		public AuthenticateResponse Authenticate(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;


			var user = _context.AspNetUsers.SingleOrDefault(x => x.UserName == username && x.PasswordHash == password);

			// detect user exists
			if (user == null) return null;

			////detect password is correct

			//if (!VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash))) return null;

			// authentication successful so generate jwt token
			var token = GenerateJwtToken(user);

			// authentication successful
			return new AuthenticateResponse(user, token);
		}

		private string GenerateJwtToken(AspNetUsers user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task<IEnumerable<UserModel>> GetAll()
		{
			var list = await _context.AspNetUsers.AsNoTracking().ToListAsync();
			return _mapper.Map<IEnumerable<UserModel>>(list);
		}

		public AspNetUsers GetById(string id)
		{
			return _mapper.Map<AspNetUsers>(_context.AspNetUsers.AsNoTracking().FirstOrDefault(x => x.Id == id));
		}

		public AspNetUsers Create(AspNetUsers user, string password)
		{
			if (string.IsNullOrWhiteSpace(password)) throw new AppException("Password is required");

			if (_context.AspNetUsers.Any(x => x.UserName == user.UserName)) throw new AppException("Username \"" + user.UserName + "\" is already taken");

			//byte[] passwordHash;
			//CreatePasswordHash(password, out passwordHash);

			//user.PasswordHash = Convert.ToBase64String(passwordHash);
			user.PasswordHash = password;
			user.Id = (Guid.NewGuid()).ToString();
			_context.AspNetUsers.Add(user);
			_context.SaveChanges();

			return user;
		}


		public void Register([FromBody]RegisterModel model)
		{
			var user = _mapper.Map<AspNetUsers>(model);
			Create(user, model.Password);
		}
	}
}
