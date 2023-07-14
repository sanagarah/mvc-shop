using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_shop.Models.ViewModels
{
	public class HomeVM
	{
        public HomeVM()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
        }
        public required IEnumerable<Product> Products { get; set; }
        public required IEnumerable<Category> Categories { get; set; }
    }
}

