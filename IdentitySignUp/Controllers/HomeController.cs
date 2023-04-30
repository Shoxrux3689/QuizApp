using IdentitySignUp.Entities;
using IdentitySignUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace IdentitySignUp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext db;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            db = appDbContext;
        }

        public IActionResult Index()
        {
            //var result = HttpContext.User;

            //var username = result.FindFirstValue(ClaimTypes.Name);

            //ViewBag.Username = username;
            
            List<User> users = db.Users.ToList<User>();
            ViewBag.UserRoles = db.UserRoles.ToList();
            //List<IdentityUserRole<Guid>> r = db.UserRoles.ToList();

            return View(users);
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