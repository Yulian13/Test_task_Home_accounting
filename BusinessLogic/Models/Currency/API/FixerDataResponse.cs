using System.Text.Json.Serialization;

namespace BusinessLogic.Models.Currency.API
{
	public class FixerDataResponse
	{
		[JsonPropertyName("rates")]
		public Dictionary<string, decimal> Rates { get; set; }
	}
}
