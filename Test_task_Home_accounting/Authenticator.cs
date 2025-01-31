using BusinessLogic.Models;
using BusinessLogic.Models.User.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Test_task_Home_accounting
{
	public class Authenticator
	{
		private readonly string Issuer;
		private readonly string Audience;
		private readonly int Duration;
		private readonly string Key;

		#region Method: Public

		public Authenticator(IOptions<AppSettings> appSettings)
		{
			var tokenSettings = appSettings.Value.JWT;
			Issuer = tokenSettings.ValidIssuer;
			Audience = tokenSettings.ValidAudience;
			Duration = tokenSettings.AccessDuration;
			Key = tokenSettings.Secret ?? throw new InvalidOperationException("Secret Key not configured");
		}

		public static void AddAuthentication(IServiceCollection services, IConfiguration configuration) =>
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(jwtOptions =>
			{
				jwtOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidAudience = configuration["JWT:ValidAudience"],
					IssuerSigningKey = GetSymmetricSecurityKey(configuration["JWT:Secret"]),
					ClockSkew = new TimeSpan(0, 0, 5)
				};
			});

		public TokensResponse GenerateTokens(Guid userId)
			=> GenerateTokens(userId, GenerateRefreshToken());

		public TokensResponse GenerateTokens(Guid userId, string refreshToken)
		{
			var accessToken = GenerateAccessToken(userId);
			return new TokensResponse()
			{
				AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
				Expiration = accessToken.ValidTo,
				RefreshToken = refreshToken
			};
		}

		public Guid GetUserIdFromAccessToken(string token)
		{
			var validation = new TokenValidationParameters
			{
				ValidIssuer = Issuer,
				ValidAudience = Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
				ValidateLifetime = false
			};
			var identityName = new JwtSecurityTokenHandler().ValidateToken(token, validation, out _).Identity.Name;
			return identityName != null ? new Guid(identityName) : Guid.Empty;
		}

		#endregion

		#region Method: Private

		private JwtSecurityToken GenerateAccessToken(Guid userId)
		{
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, userId.ToString()),
			};
			var key = GetSymmetricSecurityKey(Key);

			return new JwtSecurityToken(
				issuer: Issuer,
				audience: Audience,
				expires: DateTime.UtcNow.AddSeconds(Duration),
				claims: authClaims,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
		}

		private static SymmetricSecurityKey GetSymmetricSecurityKey(string key) =>
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

		private string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var generator = RandomNumberGenerator.Create();
			generator.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		#endregion
	}
}
