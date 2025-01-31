using BusinessLogic.Models.Expense.Arguments;
using BusinessLogic.Models.Expense.Response;
using DataBase.Models.Models;
using DataBase.Service;

namespace BusinessLogic.Services
{
	public class ExpenseEditor : BaseDBService
	{
		public ExpenseEditor(HomeAccountContext db)
			: base(db) { }

		#region Method : Public

		public async Task<Guid> Insert(InsertExpenseModel model, Guid curentUserId)
		{
			ValidateCost(model.Cost);
			var category = await GetCategory(model.CategoryId);
			var user = await GetUser(curentUserId);
			var newSpend = new Expense()
			{
				Category = category,
				Cost = model.Cost,
				Comment = model.Comment,
				CreatedBy = user,
				CreatedOn = DateTime.Now.ToUniversalTime()
			};

			DBcontext.Expenses.Add(newSpend);
			await DBcontext.SaveChangesAsync();

			return newSpend.Id;
		}

		public async Task<UpdateExpenseResponse> Update(UpdateExpenseModel model, Guid curentUserId)
		{
			var expense = await GetExpense(model.Id);
			ValidateCurrentUser(curentUserId, expense);
			if (model.Cost != null)
			{
				ValidateCost((int)model.Cost);
				expense.Cost = (int)model.Cost;
			}
			if (model.CategoryId != null)
			{
				expense.Category = await GetCategory((Guid)model.CategoryId);
			}
			if (model.Comment != null)
			{
				expense.Comment = model.Comment;
			}

			await DBcontext.SaveChangesAsync();
			return new UpdateExpenseResponse(expense);
		}

		public async Task Delete(DeleteExpenseModel model, Guid curentUserId)
		{
			var expense = await GetExpense(model.Id);
			ValidateCurrentUser(curentUserId, expense);

			DBcontext.Expenses.Remove(expense);
			await DBcontext.SaveChangesAsync();
		}

		#endregion

		#region Method : Private

		private void ValidateCost(int cost)
		{
			if (cost < 0)
			{
				throw new ArgumentException("cost is incorrect");
			}
		}

		private void ValidateCurrentUser(Guid curentUserId, Expense expense)
		{
			if (curentUserId == expense.CreatedById)
			{
				throw new ArgumentException("User cannot carry out operations with his expenses");
			}
		}

		#endregion
	}
}
