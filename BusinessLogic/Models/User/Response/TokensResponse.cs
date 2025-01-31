namespace BusinessLogic.Models.User.Response
{
	public class TokensResponse
	{
		public string AccessToken { get; set; }
		public DateTime Expiration { get; set; }
		public string RefreshToken { get; set; }
	}
}
