using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using product_Auth_DotNet_Core.Models;

namespace product_Auth_DotNet_Core.Controllers
{
    [Authorize(Roles = "User")]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("index")]
        public IActionResult Index(string value)
        {
            ViewBag.Username = value;
           

            return View(_context.Products.ToList());
        }

        [HttpGet("userinfo")]
        public IActionResult Userinfo()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Find(userId);
            
            return View(user);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
