using BusinessLogic.Helpers;
using BusinessLogic.Models.User.Arguments;
using DataBase.Models.Models;
using DataBase.Service;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
	public class UserAuthorizer : BaseDBService
	{
		public UserAuthorizer(HomeAccountContext db)
			: base(db) { }

		#region Method : Public

		public async Task Register(RegistrationModel model)
		{
			if (await DBcontext.Users.AnyAsync(user => user.Login == model.Login))
			{
				throw new ArgumentException($"Login \"{model.Login}\" is busy");
			}

			Hasher.CreatePasswordHash(model.Password, out var passwordHash, out var saltHash);
			var SecurityData = new SecurityData()
			{
				PasswordHash = passwordHash,
				PasswordSalt = saltHash,
			};

			var newUser = new User
			{
				Name = model.Username,
				Login = model.Login,
				SecurityData = SecurityData,
			};

			DBcontext.Users.Add(newUser);
			await DBcontext.SaveChangesAsync();
		}

		public bool IsVerifyLoginData(LoginModel model, ref Guid userId)
		{
			var user = DBcontext.Users.FirstOrDefault(user => user.Login == model.Login);
			if (user == null)
			{
				return false;
			}

			var isCurrentPassword = Hasher.VerifyPasswordHash(model.Password, user.SecurityData.PasswordHash, user.SecurityData.PasswordSalt);
			if (!isCurrentPassword)
			{
				return false;
			}

			userId = user?.Id ?? default;
			return true;
		}

		public bool IsVerifyRefreshToken(Guid userId, string refreshToken)
		{
			var user = GetUser(userId).Result;
			return Hasher.VerifyRefreshToken(refreshToken, user.SecurityData.RefreshTokenHash);
		}

		public async Task UpdateUserRefreshToken(Guid userId, string refreshToken)
		{
			var user = await GetUser(userId);
			user.SecurityData.RefreshTokenHash = Hasher.HashRefreshToken(refreshToken);
			await DBcontext.SaveChangesAsync();
		}

		#endregion
	}
}
