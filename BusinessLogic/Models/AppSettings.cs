namespace BusinessLogic.Models
{
	public class AppSettings
	{
		public ConnectionStrings ConnectionStrings { get; set; }
		public JWT JWT { get; set; }
		public CurrentApi CurrentApi { get; set; }
	}

	public class ConnectionStrings
	{
		public string DefaultConnection { get; set; }
	}

	public class JWT
	{
		public string ValidAudience { get; set; }
		public string ValidIssuer { get; set; }
		public int AccessDuration { get; set; }
		public string Secret { get; set; }
	}

	public class CurrentApi
	{
		public string FixerApiKey { get; set; }
	}
}
