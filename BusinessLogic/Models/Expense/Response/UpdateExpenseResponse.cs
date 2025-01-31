namespace BusinessLogic.Models.Expense.Response
{
	public class UpdateExpenseResponse
	{
		public UpdateExpenseResponse(DataBase.Models.Models.Expense expense)
		{
			Id = expense.Id;
			CategoryId = expense.CategoryId;
			Cost = expense.Cost;
			Comment = expense.Comment;
			CreatedById = expense.CreatedById;
			CreatedOn = expense.CreatedOn;
		}

		public Guid Id { get; set; }

		public Guid CategoryId { get; set; }

		public int Cost { get; set; }

		public string? Comment { get; set; }

		public Guid CreatedById { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}
