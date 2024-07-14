using DataAccessLayer.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using e_ticaret.Models;
using EntityLayer.Entity;
using System.Reflection.Metadata.Ecma335;


namespace e_ticaret.ViewComponents
{
	public class UserChartShow:ViewComponent
	{
		private readonly TContext context;
		
		public UserChartShow(TContext context)
		{
			this.context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(string email, CartProductsViewModel? newurun)
		{

			var list = await context.Charts.Include(x => x.Urun)
				.Include(x => x.Urun.urunImages).Include(x=>x.ChartFeatures).Where(x => x.User.Email == email).ToListAsync();

			List<CartProductsViewModel> uruns = new List<CartProductsViewModel>();
			
				foreach (var cart in list)
				{
					CartProductsViewModel Urun = new CartProductsViewModel();
				    Urun.ID = cart.ID;
					Urun.urunID = cart.urunID;
					Urun.Name = cart.Urun.Name;
					Urun.Number = cart.count;
					Urun.Price = cart.Urun.Price;
					Urun.ImageUrl = cart.Urun.urunImages.First().ImageUrl;
					Urun.FeaturesID = cart.ChartFeatures.Select(x=>x.featureID).ToArray();
					uruns.Add(Urun);
				}
			
			if (newurun != null)
			{
				var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
				var id = user.Id;
				var Chart = list.Where(x => x.urunID == newurun.urunID & x.userID == id).ToList();
				chart ch;
				ChartFeature cf;
				int index = 0;
				if (Chart.Count ==0)
				{
					ch = new chart()
					{
						userID = id,
						urunID = newurun.urunID,
						count = newurun.Number

					};

					context.Charts.Add(ch);
					await context.SaveChangesAsync();
					foreach (var item in newurun.FeaturesID)
					{
						cf = new ChartFeature()
						{
							chartID = ch.ID,
							featureID = item
						};
						context.ChartFeatures.Add(cf);
					}
					
					
					uruns.Add(newurun);
					
				}
				else
				{
					List<chart> charts = Chart;

					var control = charts.FirstOrDefault(x => newurun.FeaturesID
					.SequenceEqual(x.ChartFeatures.Select(x => x.featureID)));
					//bool result=control.SequenceEqual(newurun.FeaturesID);
					if (control!=null)
					{
						control.count += newurun.Number;
						var u = uruns.FirstOrDefault(x => x.ID == control.ID);
						if (u != null)
						{
							index = uruns.IndexOf(u);
							u.Number += newurun.Number;
							uruns[index] = u;
						}
						context.Charts.Update(control);
					}
					else
					{
						ch = new chart()
						{
							userID = id,
							urunID = newurun.urunID,
							count = newurun.Number

						};

						context.Charts.Add(ch);
						await context.SaveChangesAsync();
						foreach (var item in newurun.FeaturesID)
						{
							cf = new ChartFeature()
							{
								chartID = ch.ID,
								featureID = item
							};
							context.ChartFeatures.Add(cf);
						}


						uruns.Add(newurun);
					}
				}

				await context.SaveChangesAsync();

			}

			return View(uruns);
		}
	}
}
