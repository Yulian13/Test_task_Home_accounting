using BusinessLogic.Models.Report.Arguments;

namespace BusinessLogic.Models.Report.Response
{
	public class GetExpensesResponse
	{
		public GetExpensesModel Request { get; set; }
		public ExpenseStatistic[] Expenses { get; set; }
	}
}
