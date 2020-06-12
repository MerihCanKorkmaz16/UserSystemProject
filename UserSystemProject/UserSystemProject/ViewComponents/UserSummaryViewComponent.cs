using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using UserSystemProject.Identity;
using UserSystemProject.Models.Account;

namespace UserSystemProject.ViewComponents
{
    public class UserSummaryViewComponent : ViewComponent
    {
        private UserManager<AppIdentityUser> _userManager;
        public UserSummaryViewComponent(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
           
        }
        public ViewViewComponentResult Invoke()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            AppIdentityUser user = _userManager.FindByIdAsync(userId).Result;
            UserDetailsViewModel model = new UserDetailsViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(model);
        }


    }
}
