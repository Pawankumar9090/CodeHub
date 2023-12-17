using Attendance.Models;
using CodeHub.Data;
using CodeHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace NewWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<UserAccounts> _SignInManager;
        private readonly UserManager<UserAccounts> _userManager;
        private readonly MyDbContext _dbContect;

        public HomeController(ILogger<HomeController> logger, SignInManager<UserAccounts> SignInManager, MyDbContext dbContect, UserManager<UserAccounts> userManager)
        {
            _logger = logger;
            _SignInManager=SignInManager;
            _dbContect=dbContect;
            _userManager=userManager;
        }

        public IActionResult Index()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                TempData["SelectNaveIteam"] = "Question";
                return RedirectToAction("Question", "QuestionsManage");
            }
            ViewData["totalQ"]=_dbContect.Questions.Count().ToString();
            ViewData["totalUser"]=_dbContect.Users.Count();
            TempData["SelectNaveIteam"] = "Home";
            return View();
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