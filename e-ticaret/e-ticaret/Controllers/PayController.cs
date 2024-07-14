using DataAccessLayer.Connections;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
	public class PayController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly TContext context;
		

		public PayController(UserManager<AppUser> userManager, TContext context)
		{
			this.context = context;
			this._userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Index()
		{
			var email=User.Claims.SingleOrDefault(x=>x.Type==ClaimTypes.Email).Value;
			var user=await _userManager.FindByEmailAsync(email);
			var chart=await context.Charts.Include(x=>x.ChartFeatures).ThenInclude(x=>x.Feature)
				.Include(x=>x.Urun).Where(x=>x.userID==user.Id).ToListAsync();

			var sell = new sales()
			{
				userID = user.Id,
				SellBy = Convert.ToDateTime(DateTime.Now.ToString("dd-MM-yy HH:mm")),
				state = true,
			};
			context.Sales.Add(sell);
			await context.SaveChangesAsync();

			foreach (var item in chart)
			{
				if (item.ChartFeatures.Count > 1) 
				{
					int[] fID=item.ChartFeatures.Select(x=>x.featureID).ToArray();
					foreach (var ID in fID)
					{
						salesDetails sd = new salesDetails()
						{
							salesID = sell.ID,
							urunID = item.urunID,
							FeatureID = ID,
							GroupID = item.ID,
							price = item.Urun.Price,
							discount = item.Urun.Discount,
							pience = item.count,
							state = true
						};

						context.SalesDetails.Add(sd);
					}

				}
				else
				{
					salesDetails sd = new salesDetails()
					{
						salesID = sell.ID,
						urunID = item.urunID,
						FeatureID =item.ChartFeatures.Select(x=>x.featureID).First(),
						GroupID = item.ID,
						price = item.Urun.Price,
						discount = item.Urun.Discount,
						pience = item.count,
						state = true
					};

					context.SalesDetails.Add(sd);
				}

				await context.SaveChangesAsync();

			}



			return Ok();
		}
	}
}
