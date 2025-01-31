using BusinessLogic.Models.Report.Arguments;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class ReportController : HomeAccountControllerBase
	{
		private ReportService reporter { get; }

		public ReportController(ReportService reporter)
		{
			this.reporter = reporter;
		}

		/// <summary>
		/// Получить все затраты за указанный в дате месяц
		/// </summary>
		[HttpPost("expenses")]
		public async Task<IActionResult> GetExpenses([FromBody] GetExpensesModel model)
		{
			var validResponse = ValidateInputData("Incorrect getExpenses data",
				() => model.SelectMonth == DateTime.MinValue,
				() => model.Filters?.Categories != null && model.Filters.Categories.Any(id => id == Guid.Empty),
				() => model.Filters?.Users != null && model.Filters.Users.Any(id => id == Guid.Empty),
				() => model.Page != null && model.Page < 0);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await reporter.GetExpenses(model),
				"Failed to get expenses: {0}");
		}

		/// <summary>
		/// Получить статистику по категориям затрат и пользователям для конкретного периода
		/// </summary>
		[HttpPost("statistic")]
		public async Task<IActionResult> GetStatistic([FromBody] GetStatisticModel model)
		{
			var validResponse = ValidateInputData("Incorrect getStatistic data",
				() => model.EndDate == DateTime.MinValue,
				() => model.Filters?.Categories != null && model.Filters.Categories.Any(id => id == Guid.Empty),
				() => model.Filters?.Users != null && model.Filters.Users.Any(id => id == Guid.Empty));
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await reporter.GetStatistic(model),
				"Failed to get statistic: {0}");
		}
	}
}
