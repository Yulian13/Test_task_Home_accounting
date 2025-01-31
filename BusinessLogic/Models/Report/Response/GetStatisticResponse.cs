using BusinessLogic.Models.Report.Arguments;

namespace BusinessLogic.Models.Report.Response
{
	public class GetStatisticResponse
	{
		public GetStatisticModel Request { get; set; }

		public CostCategoryStatistic[] CostCategories { get; set; }

		public UserStatistic[] Users { get; set; }
	}
}
