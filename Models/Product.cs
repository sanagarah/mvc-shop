using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_shop.Models
{
	public class Product
	{
		[Key]
		public  int Id { get; set; }
		[Required]
		public string Name { get; set; } = default!;
		public string Description { get; set; } = default!;
        [Range(1,int.MaxValue)]
		public double Price { get; set; } = default!;
        public string Image { get; set; } = default!;

        public int CategoryId { get; set; } = default!;
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = default!;

    }
}

