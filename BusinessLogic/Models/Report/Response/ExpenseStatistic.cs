using DataBase.Models.Models;

namespace BusinessLogic.Models.Report.Response
{
	public class ExpenseStatistic
	{
		public ExpenseStatistic(DataBase.Models.Models.Expense expense)
		{
			ExpenseId = expense.Id;
			CreatedOn = expense.CreatedOn;
			Comment = expense.Comment;
			Cost = expense.Cost;
			Category = expense.Category;
			User = new DataUser(expense.CreatedBy);
		}

		public Guid ExpenseId { get; set; }

		public CostCategory Category { get; set; }

		public DataUser User { get; set; }

		public DateTime CreatedOn { get; set; }

		public int Cost { get; set; }

		public string? Comment { get; set; }

		#region SubClass

		public class DataUser
		{
			public DataUser(DataBase.Models.Models.User User)
			{
				Id = User.Id;
				Login = User.Login;
				Name = User.Name;
			}

			public Guid Id { get; set; }

			public string Login { get; set; }

			public string Name { get; set; }
		}

		#endregion
	}
}
