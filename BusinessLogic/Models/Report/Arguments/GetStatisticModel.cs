namespace BusinessLogic.Models.Report.Arguments
{
	public class GetStatisticModel
	{
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public AdditionalFilters? Filters { get; set; }
	}
}
