using DataAccessLayer.Connections;
using e_ticaret.Models;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
	[AllowAnonymous]
	public class CartController : Controller
	{
		private readonly TContext _context;
		private readonly UserManager<AppUser> userManager;
		public CartController(TContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			
			return View();
		}

		[HttpGet]
		public IActionResult DeleteCartShow()
		{

			if (!User.Identity.IsAuthenticated)
				return ViewComponent("CartShow");

			string Email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;


			return ViewComponent("UserChartShow", new
			{
				email = Email
			});
		}
		
		[HttpGet]

	      
		public async Task<IActionResult> CartShow(int id, int number, string featuresID)
		{

			string Email;
			int[] FeaID = JsonConvert.DeserializeObject<int[]>(featuresID);

			var urun = await _context.Uruns.Include(x => x.urunImages).FirstOrDefaultAsync(x => x.ID == id);
			var featureName = await _context.Features.Where(x => FeaID.Contains(x.ID)&x.property!=null).Select(x => x.property).ToArrayAsync();
			try
			{
				CartProductsViewModel newUrun = new CartProductsViewModel()
				{
					urunID = urun.ID,
					Name = urun.Name,
					Price = urun.Price,
					ImageUrl = urun.urunImages.First().ImageUrl,
					Number = number,
					FeaturesID = FeaID.ToArray(),
					FeaturesName = featureName.ToArray()

				};

				if (!User.Identity.IsAuthenticated)
					return ViewComponent("CartShow", newUrun);

				Email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;


				return ViewComponent("UserChartShow", new { email = Email, newurun = newUrun });
			}
			catch (Exception e)
			{

				Console.WriteLine(e.Message);
				return ViewComponent("CartShow");
			}
				
			

		}

		[HttpPost]

		public async Task<IActionResult> ProductDelete(int id)
		{

			int numbers = 0;

			if (!User.Identity.IsAuthenticated)
			{
				var list = Request.Cookies["mycart"];
				if (list != null)
				{
					var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);
					var u = uruns.FirstOrDefault(x => x.ID == id);
					if (u != null)
					{
						uruns.Remove(u);
						if (uruns.Count > 0)
						{
							var jsonUruns = JsonConvert.SerializeObject(uruns);
							var coookieOptions = new CookieOptions()
							{
								HttpOnly = true,
								Secure = true,
								Expires = DateTime.UtcNow.AddMinutes(10)
							};

							HttpContext.Response.Cookies.Append("mycart", jsonUruns, coookieOptions);
						}
						else
						{
							var coookieOptions = new CookieOptions()
							{
								HttpOnly = true,
								Secure = true,
								Expires = DateTime.UtcNow.AddMinutes(-1)
							};
							HttpContext.Response.Cookies.Append("mycart", "", coookieOptions);
						}
						//return ViewComponent("CartShow");

					}



					foreach (var item in uruns)
					{
						numbers += item.Number;
					}

					return Ok(new { number = numbers });

				}
				return Ok(new { number = 0 });
			}

			var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
			var Id = user.Id;
			var ch = await _context.Charts.FirstOrDefaultAsync(x => x.ID==id);

			_context.Charts.Remove(ch);
			await _context.SaveChangesAsync();

			var chartUruns = await _context.Charts.Where(x => x.userID == Id).ToListAsync();

			if (chartUruns.Count > 0)
			{
				foreach (var item in chartUruns)
				{
					numbers += item.count;
				}

			}


			return Ok(new { number = numbers });
		}


		public async Task<IActionResult> ShopingCart()
		{
			//int number = 0;
			//if (!User.Identity.IsAuthenticated)
			//{
			//	var list = Request.Cookies["mycart"];
			//	ViewData["cookie"] = list;

			//	if (list == null)
			//	{
			//		ViewData["number"] = 0;
			//		return View();
			//	}
			//	else
			//	{
			//		var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);

			//		foreach (var item in uruns)
			//		{
			//			number += item.Number;
			//		}
			//		ViewData["number"] = number;
			//		return View();
			//	}
			//}
			//var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
			//var email = user.Value.ToString();
			//var charts = await _context.Charts.Include(x => x.Urun).Where(x => x.User.Email == email).ToListAsync();

			//if (charts.Count > 0)
			//{
			//	foreach (var chart in charts)
			//	{
			//		number += chart.count;
			//	}
			//}
			//ViewData["number"] = number;
			//ViewData["chart"] = charts;
			//ViewData["user"] = email;

			List<object> cart = CartControl();
			ViewData["cookie"] = cart[0];
			ViewData["number"] = cart[1];
			ViewData["chart"] = cart[2];
			ViewData["user"] = cart[3];


			if (User.Identity.IsAuthenticated)
			{
				var userr = await userManager.FindByEmailAsync(User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value);
				var list = await _context.Charts.Include(x => x.ChartFeatures).ThenInclude(cf => cf.Feature)
					.Include(x => x.Urun.urunImages)
						.Include(x => x.Urun).Where(x => x.userID == userr.Id).ToListAsync();



				List<CartProductsViewModel> uruns = new List<CartProductsViewModel>();

				foreach (var item in list)
				{
					CartProductsViewModel urun = new CartProductsViewModel();
					urun.ID = item.ID;
					urun.urunID = item.urunID;
					urun.Number = item.count;
					urun.Name = item.Urun.Name;
					urun.ImageUrl = item.Urun.urunImages.First().ImageUrl;
					urun.FeaturesName = item.ChartFeatures.Select(x => x.Feature.property).ToArray();
					urun.Price = item.Urun.Price;

					uruns.Add(urun);
				}

				return View(uruns);

			}
			var cookie = Request.Cookies["mycart"];

			var cookieUruns=JsonConvert.DeserializeObject<List<CartProductsViewModel>>(cookie);

			return View(cookieUruns);
            
		}

		[HttpPost]

		public async Task<IActionResult> ShoppingAdd(int id)
		{
			if (!User.Identity.IsAuthenticated)
			{
				var cookie = Request.Cookies["mycart"];
				var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(cookie);
				var urun = uruns.FirstOrDefault(x => x.ID == id);
				int index = uruns.IndexOf(urun);
				urun.Number += 1;

				uruns[index] = urun;


				var json = JsonConvert.SerializeObject(uruns);
				var cookieOptions = new CookieOptions()
				{
					HttpOnly = true,
					Secure = true,
					Expires = DateTime.UtcNow.AddMinutes(10)
				};

				HttpContext.Response.Cookies.Append("mycart", json, cookieOptions);

				return Ok();
			}

			var email = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value;
			var user = await userManager.FindByEmailAsync(email);
			var Urun = await _context.Charts.Include(x => x.Urun)
			.FirstOrDefaultAsync(x => x.ID==id);

			Urun.count += 1;

			_context.Charts.Update(Urun);
			await _context.SaveChangesAsync();

			return Ok();
		}
		[HttpPost]

		public async Task<IActionResult> ShoppingRemove(int id)
		{

			if (!User.Identity.IsAuthenticated)
			{
				var cookie = Request.Cookies["mycart"];
				var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(cookie);
				var urun = uruns.FirstOrDefault(x => x.ID == id);
				int index = uruns.IndexOf(urun);
				urun.Number -= 1;

				uruns[index] = urun;

				var json = JsonConvert.SerializeObject(uruns);
				var cookieOptions = new CookieOptions()
				{
					HttpOnly = true,
					Secure = true,
					Expires = DateTime.UtcNow.AddMinutes(10)
				};

				HttpContext.Response.Cookies.Append("mycart", json, cookieOptions);

				return Ok();
			}
			var email = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value;
			var user = await userManager.FindByEmailAsync(email);
			var Urun = await _context.Charts.Include(x => x.Urun)
				.FirstOrDefaultAsync(x => x.ID == id);

			Urun.count -= 1;

			_context.Charts.Update(Urun);
			await _context.SaveChangesAsync();





			return Ok();
		}

		public List<object> CartControl()
		{
			List<object> liste=new List<object>();
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
				var charts =  _context.Charts.Include(x => x.Urun).Where(x => x.User.Email == email).ToList();

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
