using BusinessLogic.Models.User.Arguments;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthenticatorController : HomeAccountControllerBase
	{
		private UserAuthorizer authorizer { get; }
		private Authenticator authenticator { get; }

		public AuthenticatorController(UserAuthorizer authorizer, Authenticator authenticator)
		{
			this.authorizer = authorizer;
			this.authenticator = authenticator;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationModel model)
		{
			var validResponse = ValidateInputData("Incorrect Register data",
				() => string.IsNullOrWhiteSpace(model.Username),
				() => string.IsNullOrWhiteSpace(model.Password));
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await authorizer.Register(model),
				"Failed to create user: {0}",
				"User successfully created");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			Guid userId = default;
			var validResponse = ValidateInputData("Incorrect Login data",
				() => !authorizer.IsVerifyLoginData(model, ref userId));
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () =>
				{
					var Response = authenticator.GenerateTokens(userId);
					await authorizer.UpdateUserRefreshToken(userId, Response.RefreshToken);
					return Response;
				},
				"Failed to login user: {0}");
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
		{
			var userId = authenticator.GetUserIdFromAccessToken(model.AccessToken);
			var validResponse = ValidateInputData("Incorrect Refresh data",
				() => userId == Guid.Empty,
				() => !authorizer.IsVerifyRefreshToken(userId, model.RefreshToken));
			if (validResponse != null)
			{
				return validResponse;
			}

			return Ok(authenticator.GenerateTokens(userId, model.RefreshToken));
		}
	}
}
