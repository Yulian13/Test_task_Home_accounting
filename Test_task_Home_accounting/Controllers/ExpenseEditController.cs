using BusinessLogic.Models.Expense.Arguments;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class ExpenseEditController : HomeAccountControllerBase
	{
		private ExpenseEditor editor { get; }

		public ExpenseEditController(ExpenseEditor editor)
		{
			this.editor = editor;
		}

		[HttpPost("insert")]
		public async Task<IActionResult> Insert([FromBody] InsertExpenseModel model)
		{
			var validResponse = ValidateInputData("Incorrect insert data",
				() => model.CategoryId == Guid.Empty);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Insert(model, GetCurrentUserId()),
				"Failed to insert expense: {0}");
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] UpdateExpenseModel model)
		{
			var validResponse = ValidateInputData("Incorrect update data",
				() => model.Id == Guid.Empty);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Update(model, GetCurrentUserId()),
				"Failed to update expense: {0}");
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromBody] DeleteExpenseModel model)
		{
			var validResponse = ValidateInputData("Incorrect delete data",
				() => model.Id == Guid.Empty);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Delete(model, GetCurrentUserId()),
				"Failed to delete expense: {0}",
				"Successfully expense deleted");
		}
	}
}
