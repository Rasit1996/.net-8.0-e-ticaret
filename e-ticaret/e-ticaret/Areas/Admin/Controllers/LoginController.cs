using e_ticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using e_ticaret.Validations;
using FluentValidation.Results;
using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        

        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public LoginController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model,string token,CancellationToken cancellationToken)
        {
            LoginValidator validationRules = new LoginValidator();
            ValidationResult result = validationRules.Validate(model);
            if (result.IsValid)
            {
                var Client=new HttpClient();
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await Client.GetAsync("https://localhost:7010/api/Admin");
                if (response.IsSuccessStatusCode)
                {
                    var message=await response.Content.ReadAsStringAsync();
                    if (message == "başarılı")
                    {
                        AppUser? user = await userManager.FindByEmailAsync(model.email);
                        if (user == null)
                        {
                            ViewBag.hata = "Hatalı kullanıcı adı!";
                            return View();
                        }
                        var role = await userManager.GetRolesAsync(user);
                        if (role.First() == "Admin")
                        {

							if (User.Identity.IsAuthenticated)
								await signInManager.SignOutAsync();

							var SignInresult=await signInManager.PasswordSignInAsync(user, model.password,false,true);
                            if(SignInresult.Succeeded)
                            {
                                return RedirectToAction("Index", "Profile");
                            }
                            else
                            {
                                ViewBag.hata = "Şifre hatalı!";
                                return View();
                            }

                        }
                        ViewBag.hata = "Hatalı kullanıcı adı!";
                        return View();
                    }
                    return RedirectToAction("Index", "Profile");
                    
                }
                ViewBag.hata = $"hata={response.StatusCode}";
                return View();
                
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View();
        }

        public IActionResult Token()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateToken(TokenViewModel model)
        {
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.Key));
            var securityTokenKey = new JwtSecurityToken
                (issuer: model.ValidIssuer,
                audience: model.ValidAudience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            JwtSecurityTokenHandler handler=new JwtSecurityTokenHandler();

            return Ok(handler.WriteToken(securityTokenKey));
            
        }

        public async Task<IActionResult> Signout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }
    }
}
