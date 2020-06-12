using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserSystemProject.Identity;
using UserSystemProject.Models.Account;

namespace UserSystemProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppIdentityUser> _userManager;
        private SignInManager<AppIdentityUser> _signInManager;
        private RoleManager<AppIdentityRole> _roleManager;
        public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, RoleManager<AppIdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    //email confirm değilse hata fırlat viewe döndür
                    ModelState.AddModelError(string.Empty, "Lütfen hesabınızı doğrulayınız.");
                    return View(loginViewModel);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //yoksa hata ver
            ModelState.AddModelError("LogError", "Giriş başarısız kullanıcı adı veya şifrenizi kontrol ediniz.");
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("User").Result)
                {
                    AppIdentityRole role = new AppIdentityRole
                    {
                        Name = "User"
                    };

                    IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "We can't add the role!");
                        return View(registerViewModel);
                    }
                }
                _userManager.AddToRoleAsync(user, "User").Wait();
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = confirmationCode.Result },Request.Scheme);
                string message = "Hesabınızı onaylamak için lütfen tıklayınız.: <a href=\"" + callBackUrl + "\">link</a>";
                SendMail(message,registerViewModel.Email,"Hesap Doğrulama");
                TempData.Add("message","Kayıt başarıyla oluşturuldu,mail adresinizi kontrol ediniz");
                return RedirectToAction("Register");
            }
            TempData.Add("message", "Kullanıcı adı veya email sisteme kayıtlı giriş yapınız!");
            return View(registerViewModel);
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Login");
            }
            //url coduna tıklandı gelen parametreye göre userID si alındı

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("Unable to find the user");
            }
            //user ıd olunca code ve emaile göre confirm edildi
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                //başarılı mesajı verdirttik
                return View("ConfirmEmail");
            }
            return RedirectToAction("Index", "Student");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
            }
            //email göre arattık
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData.Add("message", "Mail adresine kayıtlı bir kullanıcı bulunamadı!");
                return View();
            }
            //reset kodu üretttik
            var confirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            //url oluşturduk
            var callBackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = confirmationCode }, Request.Scheme);
            string message = "Şifrenizi sıfırlamak için lütfen tıklayınız.: <a href=\"" + callBackUrl + "\">link</a>";
            SendMail(message, email,"Şifre Sıfırlama");
            TempData.Add("message","Sıfırlama linki mail adresinize gönderildi kontrol ediniz.");
            return View();
        }
        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new ApplicationException("User ıd or Code Code must be supplied for password reset");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                TempData.Add("message","Girmiş olduğunuz mail adresine ait kullanıcı bulunamadı!");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);

            if (result.Succeeded)
            {
                TempData["message"] = "Şifreniz başarıyla sıfırlandı";
                return RedirectToAction("Login");
            }
            else
            {
                TempData.Add("message", "Şifre sıfırlama başarısız oldu! Mail adresinizi yanlış yazmış olabilirsiniz");
            }
            return View();
        }
        
        public ActionResult LogOff()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }
        public void SendMail(string message, string useremail,string konu)
        {

            MailMessage mail = new MailMessage(); //yeni bir mail nesnesi Oluşturuldu.
            mail.IsBodyHtml = true; //mail içeriğinde html etiketleri kullanılsın mı?
            mail.To.Add(useremail); //Kime mail gönderilecek.

            //mail kimden geliyor, hangi ifade görünsün?
            mail.From = new MailAddress("destektezsecimsistemi@gmail.com", konu, System.Text.Encoding.UTF8);
            mail.Subject = konu;

            //mailin içeriği.. Bu alan isteğe göre genişletilip daraltılabilir.
            mail.Body = message;
            mail.IsBodyHtml = true;
            SmtpClient smp = new SmtpClient();

            //mailin gönderileceği adres ve şifresi
            smp.Credentials = new NetworkCredential("destektezsecimsistemi@gmail.com", "dpu12345");
            smp.Port = 587;
            smp.Host = "smtp.gmail.com";//gmail üzerinden gönderiliyor.
            smp.EnableSsl = true;
            smp.Send(mail);//mail isimli mail gönderiliyor

        }
    }
}
