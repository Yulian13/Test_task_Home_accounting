using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	public class HomeAccountControllerBase : ControllerBase
	{
		protected virtual IActionResult? ValidateInputData(string errorMessage, params Func<bool>[] predicates) =>
			predicates.Any(prd => prd())
				? StatusCode(
					StatusCodes.Status400BadRequest,
					errorMessage)
				: default;

		protected virtual async Task<IActionResult> GetResponse<T>(Func<Task<T>> asyncFunc, string errorMessageFormer)
		{
			T result = default;
			try
			{
				result = await asyncFunc();
			}
			catch (ArgumentException ex)
			{
				return StatusCode(
					StatusCodes.Status400BadRequest,
					string.Format(errorMessageFormer, ex.Message));
			}

			return Ok(result);
		}

		protected virtual async Task<IActionResult> GetResponse(Func<Task> asyncFunc, string errorMessageFormer, string successMessage)
		{
			try
			{
				await asyncFunc();
			}
			catch (ArgumentException ex)
			{
				return StatusCode(
					StatusCodes.Status400BadRequest,
					string.Format(errorMessageFormer, ex.Message));
			}

			return Ok(successMessage);
		}

		protected virtual Guid GetCurrentUserId() =>
			new Guid(HttpContext.User.Identity.Name);
	}
}
