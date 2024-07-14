using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace e_ticaret.Controllers
{
	[AllowAnonymous]
	public class LoginController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;


		public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) { 
			_signInManager = signInManager;
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
				return View();
			}
			
		}

		[HttpPost]
		public async Task<IActionResult> Index(LoginViewModel log,CancellationToken cancellationToken)
		{
			LoginValidator validator=new LoginValidator();
			ValidationResult result=validator.Validate(log);
            if (!result.IsValid)
            {
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
				return View(log);
            }

			var user = await _userManager.FindByEmailAsync(log.email);
			if (user==null)
			{
				ViewBag.hata = "Böyle bir kullanıcı yok!";
				return View(log);
			}


            var userResult = await _signInManager.PasswordSignInAsync(user, log.password, false, true);
		
			if (userResult.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else if(userResult.IsLockedOut)
			{
				ViewBag.hata = "Kullanıcı hesabı kilitlenmiş.";
				return View(log);
			}
			else
			{
				ViewBag.hata = "Kullanıcı adı veya şifre hatalı";
				return View(log);
			}
        }

		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index","Home");
		}
	}
}
