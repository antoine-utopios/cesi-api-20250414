using System.Diagnostics;
using Demo.Xss.Models;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Xss.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("PostMessage")]
        public IActionResult PostMessage(string message)
        {
            var rawMessage = message;

            var sanitizer = new HtmlSanitizer();

            var sanitizedMessage = sanitizer.Sanitize(message);

            ViewBag.Message = sanitizedMessage;
            ViewBag.RawMessage = rawMessage;

            HttpContext.Response.Headers.Append("X-Frame-Options", "DENY");
            HttpContext.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            HttpContext.Response.Headers.Append("X-Content-Type-Options", "nosniff");

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
