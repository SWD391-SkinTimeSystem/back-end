using Microsoft.AspNetCore.Mvc;

namespace SkinTime.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
