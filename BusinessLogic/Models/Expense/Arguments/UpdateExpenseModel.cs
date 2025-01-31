namespace BusinessLogic.Models.Expense.Arguments
{
	public class UpdateExpenseModel
	{
		public Guid Id { get; set; }

		public Guid? CategoryId { get; set; }

		public int? Cost { get; set; }

		public string? Comment { get; set; }
	}
}
