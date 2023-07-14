using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_shop.Models;
using mvc_shop.Models.ViewModels;
using mvc_shop.Utility;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mvc_shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db) {
            _db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Products.Include(u => u.Category),
                Categories = _db.Categories
            };
            return View(homeVM);
        }

        // GET: /<controller>/
        public IActionResult Details(int? id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Products.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault()!,
                ExistsInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }

            return View(detailsVM);
        }

        // POST: /<controller>/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id) {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }

            var itemToRemove = shoppingCartList.FirstOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}


