using DataAccessLayer.Connections;
using e_ticaret.Models;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
	public class AccountController : Controller
	{
		private readonly TContext context;
		private readonly CartProductsNumber cartProductsNumber;
		private readonly UserManager<AppUser> userManager;

		public AccountController(TContext context, UserManager<AppUser> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"] = data[2];
			ViewData["user"] = data[3];

			ViewBag.Userr = user();
			return View();

		}

		public async Task<IActionResult> sideMenu()
		{
			return PartialView("sideMenu");
		}

		public async Task<IActionResult> Message()
		{
			List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"] = data[2];
			ViewData["user"] = data[3];


			var email= User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Email).Value;	

			var list=await context.Messages.Include(x=>x.SendUser).Where(x=>x.ReceiveUser.Email == email).ToListAsync();

			ViewBag.Userr = user();

			return View(list);
		}

		public async Task<IActionResult> MessageDetail(int id)
		{
			var list = await context.Messages.Include(x => x.SendUser).FirstOrDefaultAsync(x => x.ID == id);

			ViewBag.Userr = user();

			return View(list);
		}

		public   string  user()
		{

			var email = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email);
			var user =  context.Users.SingleOrDefault(x => x.Email == email.Value);
			return $"{user.UserName} {user.SurName}";
		}

		public async Task<IActionResult> Order()
		{
			List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"] = data[2];
			ViewData["user"] = data[3];

			ViewBag.Userr = user();



			string mail = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value;
			AppUser uuser=await userManager.FindByEmailAsync(mail);
			var list=await context.SalesDetails
				.Include(x=>x.sales).Include(x=>x.urun).Include(x=>x.urun.urunImages).Include(x=>x.Feature)
				.Where(x=>x.sales.userID==uuser.Id).OrderBy(x=>x.sales.SellBy)
				.GroupBy(x=>new { x.GroupID, x.salesID }).ToListAsync();

			List<CartProductsViewModel> uruns= new List<CartProductsViewModel>();
			
			foreach (var group in list)
			{
				if (group.Count() > 1)
				{
					salesDetails sales = group.First();
					CartProductsViewModel urun = new CartProductsViewModel()
					{
						urunID = sales.urunID,
						Price = sales.price,
						Number = sales.pience,
						Name = sales.urun.Name,
						ImageUrl = sales.urun.urunImages.First().ImageUrl,
						Sellby=sales.sales.SellBy
					};
					List<string> feaName = new List<string>();
					foreach (var item in group)
					{
						feaName.Add(item.Feature.property);
					}
					urun.FeaturesName=feaName.ToArray();
					uruns.Add(urun);
				}
				else
				{
					salesDetails sales = group.First();
					CartProductsViewModel urun = new CartProductsViewModel()
					{
						urunID = sales.urunID,
						Price = sales.price,
						Number = sales.pience,
						Name = sales.urun.Name,
						ImageUrl = sales.urun.urunImages.First().ImageUrl,
						Sellby = Convert.ToDateTime(sales.sales.SellBy.ToString("dd-MM-yy HH:mm"))
						
					};
					List<string> feaName = new List<string>();
					feaName.Add(sales.Feature.property);
					urun.FeaturesName=feaName.ToArray();
					uruns.Add(urun);
				}
			}
			  var MyOrder=uruns.GroupBy(x=>x.Sellby).ToList();
			return View(MyOrder);
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
