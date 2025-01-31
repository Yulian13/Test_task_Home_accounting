namespace BusinessLogic.Models.Report.Response
{
	public class UserStatistic
	{
		public UserStatistic(DataBase.Models.Models.User user)
		{
			UserId = user.Id;
			UserName = user.Name;
			UserLogin = user.Login;
		}

		public Guid UserId { get; set; }
		public string UserName { get; set; }
		public string UserLogin { get; set; }
		public int SumCost { get; set; }
	}
}
