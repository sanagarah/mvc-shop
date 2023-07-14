using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_shop.Models.ViewModels
{
	public class ProductVM
	{
        public ProductVM()
        {
            Product = new Product();
            CategoryDropDown = new List<SelectListItem>();
        }

        public required Product Product { get; set; }
        public required IEnumerable<SelectListItem> CategoryDropDown { get; set; }
	}
}

