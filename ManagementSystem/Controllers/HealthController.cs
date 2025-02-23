using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult health()
        {
            return View();
        }
    }
}
