using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystemProject.Models.Account
{
    public class UserDetailsViewModel
    {
        [Required(ErrorMessage ="Ad kısmı boş bırakılamaz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyad kısmı boş bırakılamaz.")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Lütfen Geçerli bir email adresi giriniz")]
        public string Email { get; set; }
    }
}
