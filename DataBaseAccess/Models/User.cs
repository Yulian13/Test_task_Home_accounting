using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Models.Models
{
	public class User
	{
		public Guid Id { get; set; }

		public string Login { get; set; }

		public string Name { get; set; }

		public SecurityData SecurityData { get; set; } = new SecurityData();
	}

	[ComplexType]
	public class SecurityData
	{

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		public string PasswordSalt { get; set; }

		public string? RefreshTokenHash { get; set; }
	}
}
