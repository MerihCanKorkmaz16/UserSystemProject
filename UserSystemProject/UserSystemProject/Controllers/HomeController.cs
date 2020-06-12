using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserSystemProject.Identity;
using UserSystemProject.Models.Account;

namespace UserSystemProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserManager<AppIdentityUser> _userManager;
        public HomeController(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;

        }
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profil()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            AppIdentityUser user = _userManager.FindByIdAsync(userId).Result;
            UserDetailsViewModel model = new UserDetailsViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Profil(UserDetailsViewModel userDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                AppIdentityUser user = _userManager.FindByIdAsync(userId).Result;
                user.FirstName = userDetailsViewModel.FirstName;
                user.LastName = userDetailsViewModel.LastName;
                user.Email = userDetailsViewModel.Email;
                IdentityResult result = _userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                {
                    TempData["message"]= "Bilgileriniz başarıyla güncellendi";
                    return RedirectToAction("Profil");
                }
                else
                {
                    TempData["message"] = "Girmiş olduğunuz mail e kayıtlı hesap bulunmaktadır!";
                    return View();
                }
                
            }
            return View();
        }
    }
}
