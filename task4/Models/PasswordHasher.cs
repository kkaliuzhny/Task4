using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace task4.Models
{
	public static class PasswordHasher
	{
		public static string HashPassword(string password)
		{
			byte[] salt = GenerateSalt();
			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
			argon2.Salt = salt;
			argon2.DegreeOfParallelism = 8;
			argon2.MemorySize = 65536;
			argon2.Iterations = 4;
			byte[] hash = argon2.GetBytes(32);
			byte[] hashBytes = new byte[salt.Length + hash.Length];
			Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
			Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);
			return Convert.ToBase64String(hashBytes);
		}

		public static bool VerifyPassword(string password, string storedHash)
		{
			byte[] hashBytes = Convert.FromBase64String(storedHash);
			int saltLength = 16;
			byte[] salt = new byte[saltLength];
			Buffer.BlockCopy(hashBytes, 0, salt, 0, saltLength);
			byte[] storedHashValue = new byte[hashBytes.Length - saltLength];
			Buffer.BlockCopy(hashBytes, saltLength, storedHashValue, 0, storedHashValue.Length);
			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
			argon2.Salt = salt;
			argon2.DegreeOfParallelism = 8;
			argon2.MemorySize = 65536;
			argon2.Iterations = 4;
			byte[] newHash = argon2.GetBytes(32);
			return AreHashesEqual(storedHashValue, newHash);
		}

		private static byte[] GenerateSalt()
		{
			var salt = new byte[16];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		private static bool AreHashesEqual(byte[] hash1, byte[] hash2)
		{
			if (hash1.Length != hash2.Length)
			{
				return false;
			}
			for (int i = 0; i < hash1.Length; i++)
			{
				if (hash1[i] != hash2[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
