namespace BusinessLogic.Helpers
{
	internal static class Hasher
	{
		public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
		{
			passwordSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
			passwordHash = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
		}

		public static bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt) =>
			passwordHash.Equals(BCrypt.Net.BCrypt.HashPassword(password, passwordSalt));

		public static string HashRefreshToken(string refreshToken) =>
			BCrypt.Net.BCrypt.HashPassword(refreshToken);

		public static bool VerifyRefreshToken(string refreshToken, string? hashedToken) =>
			BCrypt.Net.BCrypt.Verify(refreshToken, hashedToken);
	}
}
