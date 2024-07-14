using DataAccessLayer.Connections;
using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class MessageController : Controller
    {
        private readonly TContext ctx;

        public MessageController(TContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<IActionResult> ReceivedMessage()
        {
            var list=await ctx.Messages.Include(x=>x.SendUser).Where(x=>x.ReceiveUserID==2).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> ReceivedMessageDetail(int id)
        {
            var message = await ctx.Messages.Include(x => x.SendUser).FirstOrDefaultAsync(x => x.ID == id);
            
            return View(message);
        }

        public async Task<IActionResult> SentMessage()
        {
            var list=await ctx.Messages.Include(x=>x.ReceiveUser).Where(x=>x.SendUserID==2).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> SentMessageDetail(int id)
        {
            var list=await ctx.Messages.Include(x=>x.ReceiveUser).FirstOrDefaultAsync(x=>x.ID==id);
            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MessageViewModel model)
        {
            MessageValidator validationRules = new MessageValidator();
            ValidationResult result=validationRules.Validate(model);
            if (result.IsValid)
            {
                var user=await ctx.Users.FirstOrDefaultAsync(x=>x.Email==model.Email);
                if (user!=null)
                {
                    DateTime now=DateTime.Now;
                    Message mes = new Message()
                    {
                        SendUserID = 2,
                        ReceiveUserID = user.Id,
                        Subject = model.Subject != null ? model.Subject : "",
                        Content = model.Content,
                        SendBy = Convert.ToDateTime(now.ToString("dd-MM-yyyy HH:mm:ss")),
                        State = true
                    };

                    ctx.Messages.Add(mes);
                    await ctx.SaveChangesAsync();

                    return RedirectToAction("SentMessage");
                }

                ViewBag.hata = "Sitenizde böyle bir kullanıcı yok!";
                return View();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View();
        }


    }
}
