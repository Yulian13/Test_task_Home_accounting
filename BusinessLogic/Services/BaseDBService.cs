using DataBase.Models.Models;
using DataBase.Service;

namespace BusinessLogic.Services
{
	public class BaseDBService
	{
		protected virtual HomeAccountContext DBcontext { get; }

		public BaseDBService(HomeAccountContext db)
		{
			DBcontext = db;
		}

		protected virtual async Task<CostCategory> GetCategory(Guid id) =>
			await DBcontext.CostCategories.FindAsync(id)
				?? throw new ArgumentException("Category is not find");

		protected virtual async Task<User> GetUser(Guid id) =>
			await DBcontext.Users.FindAsync(id)
				?? throw new ArgumentException("User is not find");

		protected virtual async Task<Expense> GetExpense(Guid id) =>
			await DBcontext.Expenses.FindAsync(id)
				?? throw new ArgumentException("Expense is not find");
	}
}
