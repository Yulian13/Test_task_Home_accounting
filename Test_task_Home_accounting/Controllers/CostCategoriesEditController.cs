using BusinessLogic.Models.CostCategories.Arguments;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class CostCategoriesEditController : HomeAccountControllerBase
	{
		private CostCategoryEditor editor { get; }

		public CostCategoriesEditController(CostCategoryEditor editor)
		{
			this.editor = editor;
		}

		[HttpPost("insert")]
		public async Task<IActionResult> Insert([FromBody] InsertCostCategoriesModel model)
		{
			var validResponse = ValidateInputData("Incorrect insert data",
				() => string.IsNullOrWhiteSpace(model.Name),
				() => string.IsNullOrWhiteSpace(model.ColorHEX));
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Insert(model),
				"Failed to insert category: {0}");
		}

		[HttpPost("update")]
		public async Task<IActionResult> Update([FromBody] UpdateCostCategoriesModel model)
		{
			var validResponse = ValidateInputData("Incorrect update data",
				() => model.Id == Guid.Empty);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Update(model),
				"Failed to update category: {0}");
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromBody] DeleteCostCategoriesModel model)
		{
			var validResponse = ValidateInputData("Incorrect delete data",
				() => model.Id == Guid.Empty);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await editor.Delete(model.Id),
				"Failed to delete category: {0}",
				"Successfully category deleted");
		}
	}
}
