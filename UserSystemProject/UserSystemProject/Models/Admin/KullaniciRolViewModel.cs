using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using UserSystemProject.Identity;

namespace UserSystemProject.Models.Admin
{
    public class KullaniciRolViewModel
    {
        public List<AppIdentityUser> Users { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
        public string SelectedId { get; set; }
    }
}
