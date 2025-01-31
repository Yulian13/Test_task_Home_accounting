namespace DataBase.Models.Models
{
	public class Expense
	{
		public Guid Id { get; set; }

		public CostCategory Category { get; set; }

		public Guid CategoryId { get; set; }

		public User CreatedBy { get; set; }

		public Guid CreatedById { get; set; }

		public DateTime CreatedOn { get; set; }

		public int Cost { get; set; }

		public string? Comment { get; set; }
	}
}
