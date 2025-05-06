using Microsoft.AspNetCore.Mvc;

namespace InvManagmentAO.Controllers
{
    public class ManagmentController : Controller
    {
        public IActionResult Inventory()
        {
            return View();
        }
    }
}
