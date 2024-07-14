using DataAccessLayer.Connections;
using e_ticaret.Models;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_ticaret.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly TContext context;

		public OrderController(TContext context)
		{
			this.context = context;
		}

		public async Task<IActionResult> Index()
		{
			
			var list = await context.SalesDetails
				.Include(x => x.sales).ThenInclude(x=>x.User).Include(x => x.urun)
				.Include(x => x.urun.urunImages).Include(x => x.Feature)
				.OrderBy(x => x.sales.SellBy)
				.GroupBy(x => new { x.GroupID, x.salesID }).ToListAsync();


			//var model = list.Select(g => new SalesDetailsViewModel
			//{
			//	SellBy = g.Key.SellBy,
			//	SalesID = g.Key.salesID,
			//	SalesDetails = g.ToList()
			//}).ToList();


			List<CartProductsViewModel> uruns = new List<CartProductsViewModel>();

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
						Sellby = sales.sales.SellBy,
						Usurname=sales.sales.User.UserName,
						SalesID=sales.salesID,
						
					};
					List<string> feaName = new List<string>();
					foreach (var item in group)
					{
						feaName.Add(item.Feature.property);
					}
					urun.FeaturesName = feaName.ToArray();
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
						Sellby = Convert.ToDateTime(sales.sales.SellBy.ToString("dd-MM-yy HH:mm")),
						Usurname=sales.sales.User.UserName,
                        SalesID = sales.salesID,

                    };
					List<string> feaName = new List<string>();
					feaName.Add(sales.Feature.property);
					urun.FeaturesName = feaName.ToArray();
					uruns.Add(urun);
				}
			}
			var model = uruns.GroupBy(x => x.Sellby).ToList();

			return View(model);
		}

		public async Task<IActionResult> Detail(int id)
		{
			var list = await context.SalesDetails.Include(x => x.sales).ThenInclude(x => x.User)
				.Include(x => x.urun).ThenInclude(x => x.urunImages).Include(x=>x.Feature).
				Where(x => x.salesID == id).GroupBy(x => x.GroupID).ToListAsync();
			List<CartProductsViewModel> uruns = new List<CartProductsViewModel>();

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
						Sellby = sales.sales.SellBy,
						Usurname = sales.sales.User.Email,
						
					};
					List<string> feaName = new List<string>();
					foreach (var item in group)
					{
						feaName.Add(item.Feature.property);
					}
					urun.FeaturesName = feaName.ToArray();
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
						Sellby = Convert.ToDateTime(sales.sales.SellBy.ToString("dd-MM-yy HH:mm")),
						Usurname = sales.sales.User.Email,
						
					};
					List<string> feaName = new List<string>();
					feaName.Add(sales.Feature.property);
					urun.FeaturesName = feaName.ToArray();
					uruns.Add(urun);
				}
			}

			return View(uruns);
		}
	}
}
