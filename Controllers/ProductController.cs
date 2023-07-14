using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_shop.Models;
using mvc_shop.Models.ViewModels;
using mvc_shop.Utility;

namespace mvc_shop.Controllers
{
    //[Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext db, IWebHostEnvironment WebHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = WebHostEnvironment;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Products.Include(u => u.Category);

            return View(objList);
        }


        // GET: /<controller>/
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryDropDown = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(productVM);
            }
            productVM.Product = _db.Products.Find(id)!;
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }


        // POST: /<controller>/ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePath;

            if (productVM.Product.Id == 0)
            {
                string fileName = Guid.NewGuid().ToString();
                string fileExtention = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + fileExtention), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                productVM.Product.Image = fileName + fileExtention;
                _db.Products.Add(productVM.Product);
            }
            else {
                var objFromDb = _db.Products.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string fileExtention = Path.GetExtension(files[0].FileName);
                    var oldFilePath = Path.Combine(upload, objFromDb!.Image);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + fileExtention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + fileExtention;
                }
                else
                {
                    productVM.Product.Image = objFromDb!.Image;
                }
                _db.Products.Update(productVM.Product);
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /<controller>/
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product obj = _db.Products.Include(u => u.Category).FirstOrDefault(u => u.Id == id)!;
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST: /<controller>/
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
             Product obj = _db.Products.Find(id)!;
            if (obj == null)
            {
                return NotFound();
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Products.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
