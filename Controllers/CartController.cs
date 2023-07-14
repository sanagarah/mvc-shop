using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using mvc_shop.Models;
using mvc_shop.Models.ViewModels;
using mvc_shop.Utility;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mvc_shop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; } = default!;

       
        public  CartController(AppDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        [Authorize]
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)?.Count() > 0) {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            List<Product> prodList = _db.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

            return View(prodList);
        }


        // POST: /<controller>/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        // GET: /<controller>/
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            List<Product> prodList = _db.Products.Where(u => prodInCart.Contains(u.Id)).ToList();

            ProductUserVM productUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim!.Value)!,
                ProductList = prodList
            };

        return View(productUserVM);
        }

        // POST: /<controller>/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM productUserVM)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
        + "templates" + Path.DirectorySeparatorChar.ToString() +
        "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products: {3}

            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            return RedirectToAction(nameof(Confirmation));
        }

        // GET: /<controller>/
        public IActionResult Confirmation()
        {
            HttpContext.Session.Clear();
            return View();

        }

        // GET: /<controller>/
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart)!;
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id)!);
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}

