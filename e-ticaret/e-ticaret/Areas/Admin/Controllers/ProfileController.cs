using DataAccessLayer.Connections;
using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly TContext context;

        public ProfileController(UserManager<AppUser> _usermanager, TContext context)
        {
            this.userManager = _usermanager;
            this.context = context;
        }

        public async  Task<IActionResult> Index()
        {
            var us =await userManager.GetUsersInRoleAsync("Admin");
            var user = us.First();

            UserUpdateViewModel model = new UserUpdateViewModel()
            {
                name = user.UserName,
                surname = user.SurName,
                email = user.Email,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserUpdateViewModel model)
        {
            UserUpdateValidator validator = new UserUpdateValidator();
            ValidationResult result=validator.Validate(model);
            if (result.IsValid)
            {
                var us = await userManager.GetUsersInRoleAsync("Admin");
                var user = us.First();
                user.UserName = model.name;
                user.Email = model.email;
                user.SurName = model.surname;
                context.Users.Update(user);

                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
            ViewBag.state = "true";
            return View(model);
        }
    }
}
