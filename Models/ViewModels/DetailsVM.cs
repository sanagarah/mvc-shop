namespace mvc_shop.Models.ViewModels
{
	public class DetailsVM
	{
        public DetailsVM()
        {
            Product = new Product();
            ExistsInCart = false;
        }
        public required Product Product { get; set; }
        public required bool ExistsInCart { get; set; }
	}
}

