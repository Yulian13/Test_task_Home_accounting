using BusinessLogic.Models.CostCategories.Arguments;
using BusinessLogic.Models.CostCategories.Response;
using DataBase.Models.Models;
using DataBase.Service;
using System.Text.RegularExpressions;

namespace BusinessLogic.Services
{
	public class CostCategoryEditor : BaseDBService
	{
		public CostCategoryEditor(HomeAccountContext db)
			: base(db) { }

		#region Method : Public

		public async Task<Guid> Insert(InsertCostCategoriesModel model)
		{
			ValidColorHex(model.ColorHEX);
			var newCategory = new CostCategory()
			{
				Name = model.Name,
				ColorHEX = model.ColorHEX,
			};
			DBcontext.CostCategories.Add(newCategory);

			await DBcontext.SaveChangesAsync();
			return newCategory.Id;
		}

		public async Task<UpdateCostCategoriesResponse> Update(UpdateCostCategoriesModel model)
		{
			var category = await GetCategory(model.Id);

			if (!string.IsNullOrWhiteSpace(model.Name))
			{
				category.Name = model.Name;
			}

			if (!string.IsNullOrWhiteSpace(model.ColorHEX))
			{
				ValidColorHex(model.ColorHEX);
				category.ColorHEX = model.ColorHEX;
			}
			await DBcontext.SaveChangesAsync();

			return new UpdateCostCategoriesResponse(category);
		}

		public async Task Delete(Guid categoryId)
		{
			ValidCategoryOnUsed(categoryId);
			var category = await GetCategory(categoryId);
			DBcontext.CostCategories.Remove(category);

			await DBcontext.SaveChangesAsync();
		}

		#endregion

		#region Method : Private

		private void ValidColorHex(string colorHex)
		{
			if (!Regex.IsMatch(colorHex, @"^(#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3}))$"))
			{
				throw new ArgumentException("Incorrect Hex color");
			}
		}

		private void ValidCategoryOnUsed(Guid categoryId)
		{
			if (DBcontext.Expenses.Any(spent => spent.CategoryId == categoryId))
			{
				throw new ArgumentException($"Category is used");
			}
		}

		#endregion
	}
}
