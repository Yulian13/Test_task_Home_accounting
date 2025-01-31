using BusinessLogic.Models;
using BusinessLogic.Models.Currency.API;
using BusinessLogic.Models.Currency.Arguments;
using BusinessLogic.Models.Currency.Responce;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BusinessLogic.Services
{
	public class FixerApiHttpService
	{
		private readonly HttpClient _httpClient;
		private readonly string ApiKey;

		public FixerApiHttpService(HttpClient httpClient, IOptions<AppSettings> appSettings)
		{
			ApiKey = appSettings.Value.CurrentApi.FixerApiKey;
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://data.fixer.io/api");
		}

		#region Method : Private

		public async Task<CurrentRatesResponse> GetCurrentRates(CurrentRatesModel model) =>
			new CurrentRatesResponse()
			{
				Rates = await GetRates(model.СurrencyCodes)
			};

		public async Task<ConvertCurrencyResponse> GetConvertCurrency(ConvertCurrencyModel model)
		{
			var currencyCodes = new string[] {model.FromCurrencyCode, model.ToCurrencyCode };
			var rates = await GetRates(currencyCodes);
			ValidFixerRates(rates, currencyCodes);

			var convertAmount = model.Amount * (rates[model.ToCurrencyCode] / rates[model.FromCurrencyCode]);

			return new ConvertCurrencyResponse()
			{
				CurrencyCode = model.ToCurrencyCode,
				Amount = convertAmount,
			};
		}

		public async Task<bool> CheckServiceAndApiKey()
			=> (await GetRates(["USD"]))?.Any() ?? false;

		#endregion

		#region Method : Private

		private async Task<Dictionary<string, decimal>> GetRates(string[]? currencyCodes)
		{
			var requestUri = $"/latest?access_key={ApiKey}"
				+ (currencyCodes != null
					? $"&symbols={string.Join(',', currencyCodes)}"
					: string.Empty);
			var response = await _httpClient.GetAsync(requestUri);
			ValidFixerResponse(response);
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var ratesResponse = JsonSerializer.Deserialize<FixerDataResponse>(jsonResponse);

			return ratesResponse.Rates;
		}

		private void ValidFixerResponse(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				throw new ArgumentException("Response is not success from currency service");
			}
		}

		private void ValidFixerRates(Dictionary<string, decimal> rates, string[] currencyCodes)
		{
			var errorCodes = new List<string>();
			foreach (var code in currencyCodes)
			{
				if (!rates.TryGetValue(code, out _))
				{
					errorCodes.Add(code);
				}
			}

			if (errorCodes.Any())
			{
				throw new ArgumentException($"CurrencyCodes: {string.Join(", ", errorCodes)} is incorrect");
			}
		}

		#endregion
	}
}
