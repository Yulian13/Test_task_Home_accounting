using DataBase.Models.Models;

namespace BusinessLogic.Models.CostCategories.Response
{
	public class UpdateCostCategoriesResponse
	{
		public UpdateCostCategoriesResponse(CostCategory category)
		{
			Id = category.Id;
			Name = category.Name;
			ColorHEX = category.ColorHEX;
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string ColorHEX { get; set; }
	}
}
