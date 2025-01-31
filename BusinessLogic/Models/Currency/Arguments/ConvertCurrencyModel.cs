namespace BusinessLogic.Models.Currency.Arguments
{
	public class ConvertCurrencyModel
	{
		public string FromCurrencyCode { get; set; }
		public string ToCurrencyCode { get; set; }
		public decimal Amount { get; set; }
	}
}
