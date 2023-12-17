using Attendance.Models;
using CodeHub.Data;
using CodeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewWebApp.Controllers;
using System.Diagnostics;

namespace CodeHub.Controllers
{
    public class UserAccountController : Controller
    {       
        private readonly MyDbContext _context;
        private readonly UserManager<UserAccounts> _userManager;
        public UserAccountController(MyDbContext context, UserManager<UserAccounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult CreateUser(UserAccounts user)
        {
            if (user != null)
            {                
                user = _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User)).Result;
                UserDetails userDetails = new() { Name=user.Name,UserId = user.Id,Email=user.Email};
                _context.Add(userDetails);
                _context.SaveChanges();
            }
            TempData["SelectNaveIteam"] = "Question";
            return RedirectToAction("Question","QuestionsManage");
        }
        [Authorize(Roles ="Admin,User")]
        [HttpGet]
        public IActionResult UserProfile()
        {
            UserAccounts loginuser = _userManager.FindByIdAsync(_userManager.GetUserId(User)).Result;
            if (loginuser != null)
            {
                ViewData["user"] = _context.UserDetails.Find(loginuser.Id);
            }
            return View();
        }
        [Authorize(Roles ="Admin,User")]
        [HttpPost]
        public IActionResult UserProfile(UserDetails updateuser)
        {
            if (ModelState.IsValid && updateuser!=null)
            {
                _context.Update(updateuser);
                _context.SaveChanges();
                TempData["success"] = "Details Updated!";
            }
            return RedirectToAction("UserProfile","UserAccount");
        }

        [Authorize(Roles ="Admin,User")]
        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePassword change)
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(HttpContext.User);
                UserAccounts loginuser = _userManager.FindByIdAsync(userid).Result;
                if (change != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(loginuser, change.CurrentPassword, change.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["success"] = "Password changed";
                        RedirectToAction("UserProfile", "UserAccount");
                    }
                }
            }
            //TempData["error"] = "Current Password Incarect";
            return RedirectToAction("UserProfile","UserAccount");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
