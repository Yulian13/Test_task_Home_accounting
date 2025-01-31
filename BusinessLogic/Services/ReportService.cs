using BusinessLogic.Models.Report.Arguments;
using BusinessLogic.Models.Report.Response;
using DataBase.Models.Models;
using DataBase.Service;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
	public class ReportService : BaseDBService
	{
		private const int pageStep = 10;

		public ReportService(HomeAccountContext db) : base(db) { }

		#region Method : Public

		public async Task<GetExpensesResponse> GetExpenses(GetExpensesModel model)
		{
			var result = new GetExpensesResponse() { Request = model };
			var expenses = (await GetExpensesByAdditionalFilters(
				DBcontext.Expenses
					.Where(exp =>
						exp.CreatedOn.Year == model.SelectMonth.Year
						&& exp.CreatedOn.Month == model.SelectMonth.Month)
					.Include(exp => exp.CreatedBy)
					.Include(exp => exp.Category),
				model.Filters))
				.Select(exp => new ExpenseStatistic(exp));

			if (model.Page != null)
			{
				expenses = expenses.Skip((int)model.Page * pageStep).Take(pageStep);
			}

			result.Expenses = expenses.ToArray();
			return result;
		}

		public async Task<GetStatisticResponse> GetStatistic(GetStatisticModel model)
		{
			ValidPeriodDates(model.StartDate, model.EndDate);
			var result = new GetStatisticResponse() { Request = model };

			var expenses =  await GetExpensesByAdditionalFilters(
				DBcontext.Expenses.Where(exp =>
					exp.CreatedOn <= model.EndDate
					&& exp.CreatedOn >= model.StartDate),
				model.Filters);
			var categories = await GetCostCategoryByAdditionalFilters(model.Filters);
			var users = await GetUsersByAdditionalFilters(model.Filters);

			double sumCost = expenses.Sum(exp => exp.Cost);
			result.CostCategories = GetCategoryStatistic(categories, expenses);
			result.Users = GetUserStatistic(users, expenses);

			return result;
		}

		#endregion

		#region Method : Private

		private void ValidPeriodDates(DateTime startDate, DateTime endDate)
		{
			if (endDate <= startDate)
			{
				throw new ArgumentException("End date should be more than start date");
			}
		}

		private async Task<Expense[]> GetExpensesByAdditionalFilters(IQueryable<Expense> expenses, AdditionalFilters? filters)
		{
			if (filters?.Users != null)
			{
				expenses = expenses.Where(exp => filters.Users.Contains(exp.CreatedById));
			}
			if (filters?.Categories != null)
			{
				expenses = expenses.Where(exp => filters.Categories.Contains(exp.CategoryId));
			}
			return await expenses.ToArrayAsync();
		}

		private async Task<CostCategory[]> GetCostCategoryByAdditionalFilters(AdditionalFilters? filters)
		{
			IQueryable<CostCategory> query = DBcontext.CostCategories;
			if (filters?.Categories != null)
			{
				query = query.Where(catergoty => filters.Categories.Contains(catergoty.Id));
			}
			return await query.ToArrayAsync();
		}

		private async Task<User[]> GetUsersByAdditionalFilters(AdditionalFilters? filters)
		{
			IQueryable<User> query = DBcontext.Users;
			if (filters?.Users != null)
			{
				query = query.Where(user => filters.Users.Contains(user.Id));
			}
			return await query.ToArrayAsync();
		}

		private CostCategoryStatistic[] GetCategoryStatistic(CostCategory[] categories, Expense[] expenses)
		{
			double sumCost = expenses.Sum(exp => exp.Cost);
			return categories.Select(category =>
			{
				double categorySumCost = expenses.Where(exp => exp.CategoryId == category.Id).Sum(exp => exp.Cost);
				return new CostCategoryStatistic(category)
				{
					SumCost = (int)categorySumCost,
					Procent = sumCost > 0 ? Math.Round((categorySumCost / sumCost) * 100f, 2) : 0
				};
			})
			.ToArray();
		}

		private UserStatistic[] GetUserStatistic(User[] users, Expense[] expenses) =>
			users.Select(user =>
				new UserStatistic(user)
				{
					SumCost = expenses.Where(exp => exp.CreatedById == user.Id).Sum(exp => exp.Cost)
				})
			.ToArray();

		#endregion
	}
}
