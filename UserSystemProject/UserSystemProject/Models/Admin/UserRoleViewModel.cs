using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserSystemProject.Identity;

namespace UserSystemProject.Models.Admin
{
    public class UserRoleViewModel
    {
        public List<AppIdentityRole> Roles { get; set; }
        [Required(ErrorMessage = "Rol tipi doldurmak zorunludur!")]
        public string Name { get; set; }
        public string RolId { get; set; }
    }
}
