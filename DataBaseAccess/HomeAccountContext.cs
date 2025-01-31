using DataBase.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Service
{
	public class HomeAccountContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;

		public DbSet<CostCategory> CostCategories { get; set; } = null!;

		public DbSet<Expense> Expenses { get; set; } = null!;

		public HomeAccountContext(DbContextOptions<HomeAccountContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
