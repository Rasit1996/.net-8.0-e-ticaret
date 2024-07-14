using DataAccessLayer.Connections;
using e_ticaret.Models;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
	{
		private readonly TContext context;
		

		public CategoryController(TContext context)
		{
			this.context = context;
			
		}

		public async Task<IActionResult> Products(int id)
		{
			int number = 0;
			var list = await context.Uruns.Include(x => x.category).Include(x => x.urunImages)
				.Where(x => x.categoryID == id & x.State == true & x.urunImages.Count > 0).ToListAsync();

			ViewBag.kat = context.Categories.SingleOrDefault(x => x.ID == id).Name;
			
			List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"] = data[2];
			ViewData["user"] = data[3];

			return View(list);

		}

		[HttpPost]
		public async Task<IActionResult> QuickView(int id)
		{
			var urun=await context.Uruns.Include(x=>x.urunImages).Include(x=>x.UrunFeatures).SingleOrDefaultAsync(x=>x.ID == id);

			return ViewComponent("ProductQuickView", urun);
		}

		public PartialViewResult script()
		{
			return PartialView();
		}

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
				var charts = context.Charts.Include(x => x.Urun).Where(x => x.User.Email == email).ToList();

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
