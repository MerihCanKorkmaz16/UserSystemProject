using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using UserSystemProject.Identity;
using UserSystemProject.Models.Admin;

namespace UserSystemProject.ViewComponents
{
    public class RolGuncelleViewComponent : ViewComponent
    {
        
        public ViewViewComponentResult Invoke(UserRoleViewModel model)
        {
            return View(model);
        }
    }
}
