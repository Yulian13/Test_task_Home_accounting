namespace BusinessLogic.Models.Report.Arguments
{
	public class GetExpensesModel
	{
		public DateTime SelectMonth { get; set; }

		public int? Page { get; set; }

		public AdditionalFilters? Filters { get; set; }
	}
}
