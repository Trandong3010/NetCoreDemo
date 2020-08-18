using System;

namespace Infrastructure.Jwt
{
	public class PasswordHash
	{
		private static void CreatePasswordHash(string password, out byte[] paswordHash)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (String.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				paswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private static bool VerifyPasswordHash(string password, byte[] paswordHash)
		{
			if (password == null) throw new ArgumentNullException();
			if (String.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (paswordHash.Length != 64) throw new ArgumentException("Invalid of password hash (64 bytes expected).", "paswordHash");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				var a = Convert.ToBase64String(computedHash);
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != paswordHash[i]) return false;
				}
			}

			return true;
		}
	}
}
