using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc_shop.Models
{
	public class Category
	{
		[Key]
		public required int Id { get; set; }
		[Required]
		public required string Name { get; set; }
		[DisplayName("Display Order")]
		[Required]
		[Range(1, int.MaxValue,ErrorMessage="Enter right number")]
		public required int DisplayOrder { get; set; }
	}
}

