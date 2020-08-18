using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Common.Authentication;
using Infrastructure.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
	public interface IUserService
	{
		AspNetUsers Authenticate(string username, string password);
		Task<IEnumerable<UserModel>> GetAll();
		AspNetUsers GetById(int id);
		AspNetUsers Create(AspNetUsers user, string password);
		void Update(AspNetUsers user, string password = null);
		void Delete(string id);
		void Register([FromBody] RegisterModel model);
	}

	public class UserService : IUserService
	{
		protected readonly SchoolContext _context;
		private readonly IMapper _mapper;

		public UserService(SchoolContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public AspNetUsers Authenticate(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

			var user = _context.AspNetUsers.SingleOrDefault(x => x.UserName == username);

			// detect user exists
			if (user == null) return null;

			//detect password is correct

			if (!VerifyPasswordHash(password, Encoding.UTF8.GetBytes(user.PasswordHash))) return null;

			// authentication successful
			return user;
		}

		public async Task<IEnumerable<UserModel>> GetAll()
		{
			var list = await _context.AspNetUsers.AsNoTracking().ToListAsync();
			return _mapper.Map<IEnumerable<UserModel>>(list);
		}

		public AspNetUsers GetById(int id)
		{
			throw new NotImplementedException();
		}

		public AspNetUsers Create(AspNetUsers user, string password)
		{
			if (string.IsNullOrWhiteSpace(password)) throw new AppException("Password is required");

			if (_context.AspNetUsers.Any(x => x.UserName == user.UserName)) throw new AppException("Username \"" + user.UserName + "\" is already taken");

			byte[] passwordHash;
			CreatePasswordHash(password, out passwordHash);

			user.PasswordHash = Convert.ToBase64String(passwordHash);
			user.Id = (Guid.NewGuid()).ToString();
			_context.AspNetUsers.Add(user);
			_context.SaveChanges();

			return user;
		}

		public void Update(AspNetUsers user, string password = null)
		{
			throw new NotImplementedException();
		}

		public void Delete(string id)
		{
			throw new NotImplementedException();
		}

		public void Register([FromBody] RegisterModel model)
		{
			var user = _mapper.Map<AspNetUsers>(model);
			Create(user, model.Password);
		}

		private static void CreatePasswordHash(string password, out byte[] paswordHash)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				paswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private static bool VerifyPasswordHash(string password, byte[] storedHash)
		{
			if (password == null) throw new ArgumentNullException();
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (storedHash.Length != 64) throw new ArgumentException("Invalid of password hash (64 bytes expected).", "passwordHash");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash[i]; i++)
				{
					if (computedHash[i] != storedHash[i]) return false;
				}
			}

			return true;
		}
	}
}
