using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserSystemProject.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string Code { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6}$",
         ErrorMessage = "Şifreniz 6 karakterli,içinde en az bir küçük, bir büyük ,bir rakam ve sembol içermelidir!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Tekrar şifrenizi giriniz!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6}$",
         ErrorMessage = "Şifreniz 6 karakterli,içinde en az bir küçük, bir büyük ,bir rakam ve sembol içermelidir!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email giriniz!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email adresiniz doğru formatta değil!")]
        public string Email { get; set; }
    }
}
