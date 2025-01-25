using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using product_Auth_DotNet_Core.Models;

namespace product_Auth_DotNet_Core.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/products")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string value) {
            ViewBag.Username = value;
            return View(_context.Products.ToList());
            }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost("edit")]
        public IActionResult Edit(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
