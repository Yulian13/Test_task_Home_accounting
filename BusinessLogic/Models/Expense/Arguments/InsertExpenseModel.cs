namespace BusinessLogic.Models.Expense.Arguments
{
	public class InsertExpenseModel
	{
		public Guid CategoryId { get; set; }

		public int Cost { get; set; }

		public string? Comment { get; set; }
	}
}
