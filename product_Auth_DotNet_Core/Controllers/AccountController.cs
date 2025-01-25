using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using product_Auth_DotNet_Core.Models;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace product_Auth_DotNet_Core.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) return Unauthorized();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            string value = user.Username;
            int id = user.Id;
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin", new { value = value });
            }
            else
            {
                return RedirectToAction("Index", "User", new { value = value,id=id });
            }
            

            
        }


        [HttpGet("signup")]
        public IActionResult Signup() => View();

        [HttpPost("signup")]
        public IActionResult Signup(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogoutRedirect");
        }
        public IActionResult LogoutRedirect()
        {
            return View();
        }

    }
}
