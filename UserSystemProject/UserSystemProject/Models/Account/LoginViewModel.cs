using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystemProject.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Kullanıcı adı boş bırakılamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Kullanıcı şifresi boş bırakılamaz")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
