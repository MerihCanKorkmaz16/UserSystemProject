using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSystemProject.Models.Admin;

namespace UserSystemProject.ViewComponents
{
    public class KullaniciRolDüzenleViewComponent:ViewComponent
    {
        public ViewViewComponentResult Invoke(List<KullaniciRolViewModel> assignRoles)
        {
            return View(assignRoles);
        }
    }
}
