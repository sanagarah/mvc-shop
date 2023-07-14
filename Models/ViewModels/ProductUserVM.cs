using System.Collections.Generic;
using System.Security.Claims;

namespace mvc_shop.Models.ViewModels
{
	public class ProductUserVM
	{
        public ProductUserVM()
        {
            ApplicationUser = new ApplicationUser();
            ProductList = new List<Product>();
        }

        public required ApplicationUser ApplicationUser { get; set; }
        public required IList<Product> ProductList { get; set; }
    }
}

