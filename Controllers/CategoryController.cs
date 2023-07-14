using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_shop.Models;
using mvc_shop.Utility;

namespace mvc_shop.Controllers
{
    //[Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Categories;
            return View(objList);
        }

        // GET: /<controller>/
        public IActionResult Create()
        {
            return View();
        }

        // POST: /<controller>/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        // GET: /<controller>/
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category obj = _db.Categories.Find(id)!;
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        // POST: /<controller>/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: /<controller>/
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category obj = _db.Categories.Find(id)!;
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
            Category obj = _db.Categories.Find(id)!;
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
