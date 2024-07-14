using DataAccessLayer.Connections;
using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
    [AllowAnonymous]
    public class ContactController : Controller
	{
		private readonly ICompositeViewEngine _viewEngine;
		


		TContext ctx=new TContext();
		public ContactController(ICompositeViewEngine viewEngine)
		{
			_viewEngine = viewEngine;
		   
		}


		public async Task<IActionResult> Index()
		{
			List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"] = data[2];
			ViewData["user"] = data[3];

			return View();
		}
		public PartialViewResult Form()
		{
			return PartialView();
		}

		[HttpPost]

		public async Task<IActionResult> Form(MessageViewModel mes, CancellationToken cancellationToken)
		{
			Stopwatch sw=new Stopwatch();
			
			MessageValidator mesValite = new MessageValidator();
			ValidationResult result = mesValite.Validate(mes);
			if (result.IsValid)
			{
				sw.Start();
				var user=await ctx.Users.SingleOrDefaultAsync(x=>x.Email == mes.Email);
				if (user == null) 
				{
					ViewBag.hata = "Mesaj göndermek için lütfen kaydolun";
					sw.Stop();
					TimeSpan elapsd = sw.Elapsed;
					Console.WriteLine($"çalışma süresi={elapsd.TotalMilliseconds}");
					return PartialView(mes);
				}
				DateTime now= DateTime.Now;
				Message m = new Message()
				{
					Content = mes.Content,
					State = true,
					SendBy =Convert.ToDateTime( now.ToString("dd-MM-yyyy HH:mm:ss")),
					SendUserID = user.Id,
				    ReceiveUserID=2,
					Subject=mes.Subject!=null ? mes.Subject :""
					
				};
				await ctx.Messages.AddAsync(m);
				await ctx.SaveChangesAsync();
				var partialviewresult = PartialView("Form");
				StringWriter writer = new StringWriter();
				var viewEngineResult = _viewEngine.FindView(ControllerContext, "Form", false);
				var view = viewEngineResult.View;
				var viewcontext = new ViewContext(ControllerContext, view, ViewData, TempData, writer, new HtmlHelperOptions());
				await view.RenderAsync(viewcontext);

				var viewcontent = writer.GetStringBuilder().ToString();

				//var viewcontent = html();
				sw.Stop();
				TimeSpan elapsed = sw.Elapsed;
				Console.WriteLine($"Çalışma süresi= {elapsed.TotalMilliseconds.ToString() } mili saniye");

				return Json(new { success = true, partialview = viewcontent });
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
			return PartialView("Form", mes);
		}

		//public string html()
		//{
		//	string html = "<form id=\"formsub\">\r\n\t<h4 " +
		//		"class=\"mtext-105 cl2 txt-center p-b-30\">\r\n\t\tBize Mesaj Gönder\r\n\t</h4>\r\n\t<div " +
		//		"class=\"form-group\">\r\n\t<div class=\"bor8 m-b-20 how-pos4-parent\">\r\n\t\t<input " +
		//		"class=\"stext-111 cl2 plh3 size-116 p-l-62 p-r-30\" asp-for=\"Email\" placeholder=\"Email " +
		//		"adresiniz\">\r\n\t\t<img class=\"how-pos4 pointer-none\" src=\"/cozastore-master/images/icons/" +
		//		"icon-email.png\" alt=\"ICON\">\r\n\t\t\r\n\t</div>\r\n\t<span asp-validation-for=\"Email\" " +
		//		"class=\"text-danger md\"></span>\r\n\t</div>\r\n\t<div class=\"form-group\">\r\n\t<div " +
		//		"class=\"bor8 m-b-30\">\r\n\t\t<textarea class=\"stext-111 cl2 plh3 size-120 p-lr-28 p-tb-25\" " +
		//		"asp-for=\"Content\" placeholder=\"Nasıl yardımcı olabiliriz?\"></textarea>\r\n\t\t\r\n\t</div>\r\n\t\t<span " +
		//		"asp-validation-for=\"Content\" class=\"text-danger md\"></span>\r\n\t</div>\r\n\r\n\t<button " +
		//		"type=\"submit\" class=\"flex-c-m stext-101 cl0 size-121 bg3 bor1 hov-btn3 p-lr-15 trans-04 " +
		//		"pointer\">\r\n\t\tGönder\r\n\t</button>\r\n</form>\r\n\r\n";

		//	return html;
		//}

		public List<object> CartControl()
		{
			List<object> liste = new List<object>();
			int number = 0;
			if (!User.Identity.IsAuthenticated)
			{
				var list = Request.Cookies["mycart"];
				liste.Add(list);


				if (list == null)
				{
					ViewData["number"] = 0;
					liste.Add(0);

				}
				else
				{
					var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);

					foreach (var item in uruns)
					{
						number += item.Number;
					}
					ViewData["number"] = number;
					liste.Add(number);

				}
				liste.Add(null);
				liste.Add(null);
				return liste;
			}
			else
			{

				var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
				var email = user.Value.ToString();
				var charts = ctx.Charts.Include(x => x.Urun).Where(x => x.User.Email == email).ToList();

				if (charts.Count > 0)
				{
					foreach (var chart in charts)
					{
						number += chart.count;
					}
				}
				liste.Add(null);
				liste.Add(number);
				liste.Add(charts);
				liste.Add(email);

				return liste;
			}

		}

	}
}
