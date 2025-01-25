using Microsoft.AspNetCore.Mvc;

namespace product_Auth_DotNet_Core.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
