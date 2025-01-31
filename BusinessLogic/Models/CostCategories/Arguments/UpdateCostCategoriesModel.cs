namespace BusinessLogic.Models.CostCategories.Arguments
{
	public class UpdateCostCategoriesModel
	{
		public Guid Id { get; set; }

		public string? Name { get; set; }

		public string? ColorHEX { get; set; }
	}
}
