using DataBase.Models.Models;

namespace BusinessLogic.Models.Report.Response
{
	public class CostCategoryStatistic
	{
		public CostCategoryStatistic(CostCategory category)
		{
			CategoryId = category.Id;
			CategoryName = category.Name;
			CategoryColorHex = category.ColorHEX;
		}

		public string CategoryName { get; set; }
		public Guid CategoryId { get; set; }
		public string CategoryColorHex { get; set; }
		public double Procent { get; set; }
		public int SumCost { get; set; }
	}
}
