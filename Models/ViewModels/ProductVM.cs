using Microsoft.AspNetCore.Mvc.Rendering;

namespace mvc_shop.Models.ViewModels
{
	public class ProductVM
	{
		public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryDropDown { get; set; }
	}
}

