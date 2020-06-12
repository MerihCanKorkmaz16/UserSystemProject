using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserSystemProject.Identity;
using UserSystemProject.Models.Admin;

namespace UserSystemProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private RoleManager<AppIdentityRole> _roleManager;
        private UserManager<AppIdentityUser> _userManager;
        public AdminController(RoleManager<AppIdentityRole> roleManager, UserManager<AppIdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult KullaniciRol()
        {
            UserRoleViewModel model = new UserRoleViewModel
            {
                Roles = _roleManager.Roles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciRolEkle(UserRoleViewModel model)
        {
            IdentityResult result = await _roleManager.CreateAsync(new AppIdentityRole { Name = model.Name });
            if (result.Succeeded)
            {
                TempData["message"] = "Rol başarıyla eklendi!";
                return RedirectToAction("KullaniciRol");
            }
            else
            {
                TempData["message"] = "Eklemek istediğiniz rol zaten kayıtlı!";
                return RedirectToAction("KullaniciRol");
            }

        }
        public async Task<IActionResult> KullaniciRolSilme(string rolId)
        {
            AppIdentityRole role = await _roleManager.FindByIdAsync(rolId);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                System.Threading.Thread.Sleep(500);
                return RedirectToAction("KullaniciRol");
            }
            else
            {
                return RedirectToAction("KullaniciRol");
            }
        }

        public async Task<IActionResult> RolGüncelle(string rolId)
        {
            AppIdentityRole role = await _roleManager.FindByIdAsync(rolId);
            UserRoleViewModel model = new UserRoleViewModel
            {
                Name = role.Name,
                RolId = rolId
            };
            return ViewComponent("RolGuncelle", model);
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciRolGüncelle(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityRole role = await _roleManager.FindByIdAsync(model.RolId);
                role.Name = model.Name;
                IdentityResult result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    TempData["message"] = "Rol başarıyla Güncellendi";
                    return RedirectToAction("KullaniciRol");
                }
            }
            return ViewComponent("RolGuncelle", model);
        }


        public ActionResult KullaniciDüzenle()
        {
            KullaniciRolViewModel kullaniciRolViewModel = new KullaniciRolViewModel
            {
                Users = _userManager.Users.ToList()
            };
            return View(kullaniciRolViewModel);
        }

        public async Task<IActionResult> KullaniciRolDüzenle(string userId)
        {
            AppIdentityUser user = await _userManager.FindByIdAsync(userId);
            List<AppIdentityRole> allRoles = _roleManager.Roles.ToList();
            List<string> userRoles = await _userManager.GetRolesAsync(user) as List<string>;
            List<KullaniciRolViewModel> assignRoles = new List<KullaniciRolViewModel>();
            allRoles.ForEach(role => assignRoles.Add(new KullaniciRolViewModel
            {
                IsSelected = userRoles.Contains(role.Name),
                RoleId = role.Id,
                RoleName = role.Name,
                SelectedId = user.Id
            }));
            return ViewComponent("KullaniciRolDüzenle", assignRoles);

        }


        [HttpPost]
        public async Task<ActionResult> KullaniciRolDüzenlePost(List<KullaniciRolViewModel> modelList,string id)
        {
            AppIdentityUser user = await _userManager.FindByIdAsync(id);
            foreach (KullaniciRolViewModel role in modelList)
            {
                if (role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }
            TempData.Add("message","Kullanıcının rolü değiştirildi!");
            return RedirectToAction("KullaniciDüzenle");

        }

    }

}
