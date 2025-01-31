using BusinessLogic.Models.Currency.Arguments;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test_task_Home_accounting.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class CurrencyController : HomeAccountControllerBase
	{
		private FixerApiHttpService currencyService;

		public CurrencyController(FixerApiHttpService currencyService)
		{
			this.currencyService = currencyService;
		}

		[HttpPost("rates")]
		public async Task<IActionResult> GetCurrentRates([FromBody] CurrentRatesModel model)
		{
			var validResponse = ValidateInputData("Incorrect get current data",
				() => model.СurrencyCodes != null && model.СurrencyCodes.Any(code => string.IsNullOrWhiteSpace(code)));
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await currencyService.GetCurrentRates(model),
				"Failed to get current rates: {0}");
		}

		[HttpPost("convert")]
		public async Task<IActionResult> ConvertCurrency([FromBody] ConvertCurrencyModel model)
		{
			var validResponse = ValidateInputData("Incorrect convert data",
				() => string.IsNullOrWhiteSpace(model.FromCurrencyCode),
				() => string.IsNullOrWhiteSpace(model.ToCurrencyCode),
				() => model.Amount < 0);
			if (validResponse != null)
			{
				return validResponse;
			}

			return await GetResponse(
				async () => await currencyService.GetConvertCurrency(model),
				"Failed to get convert current: {0}");
		}
	}
}
