namespace BusinessLogic.Models.User.Arguments
{
	public class RefreshModel
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
