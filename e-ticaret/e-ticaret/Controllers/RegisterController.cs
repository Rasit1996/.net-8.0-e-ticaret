using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace e_ticaret.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
			var list = Request.Cookies["mycart"];
			ViewData["cookie"] = list;

			if (list == null)
			{
				ViewData["number"] = 0;
				return View();
			}
			else
			{
				var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);
				int number = 0;
				foreach (var item in uruns)
				{
					number += item.Number;
				}
				ViewData["number"] = number;
				
			}
			return View();
        }

        [HttpPost]


        public async Task<IActionResult> Index(RegisterViewModel res)
        {
            RegisterValidater validator = new RegisterValidater();
            ValidationResult result = validator.Validate(res);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(res);
            }
            
            AppUser user = new AppUser()
            {
                UserName = res.name,
                SurName = res.surname,
                Email = res.email,

            };
            var resultCreating=await _userManager.CreateAsync(user,res.password);
            if (resultCreating.Succeeded)
            {
              await  _userManager.AddToRoleAsync(user, "uye");
                return RedirectToAction("Index","Home");
            }
            else
            {
                foreach (var error in resultCreating.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

        }



    }
}
