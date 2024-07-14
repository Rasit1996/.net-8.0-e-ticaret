using Azure;
using e_ticaret.Models;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace e_ticaret.ViewComponents
{
	
	public class CartShow:ViewComponent
	{


		public async Task<IViewComponentResult> InvokeAsync(CartProductsViewModel? urun)
		{
			var list = Request.Cookies["mycart"];
			if (list != null)
			{
				var urunsRequest = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);
				if (urun != null)
				{
			        var u = urunsRequest.Where(x => x.urunID == urun.urunID).ToList();
					//bool result;
					CartProductsViewModel result=new CartProductsViewModel();
					if (u != null)
					{
						 result = u.SingleOrDefault(x => urun.FeaturesName.SequenceEqual(x.FeaturesName));
					}
					



					int index;
					if (result!=null)
					{
						
							index = urunsRequest.IndexOf(result);
							result.Number += urun.Number;
							urunsRequest[index] = result;
						
					}
					else
					{
						int id = urunsRequest.Count + 1;
						urun.ID = id;
						urunsRequest.Add(urun);
					}
					
				}
				var jsonList=JsonConvert.SerializeObject(urunsRequest);
				var cookieOptions2 = new CookieOptions()
				{
					HttpOnly = true,
					Secure = true,
					Expires = DateTime.UtcNow.AddMinutes(10)
				};
				HttpContext.Response.Cookies.Append("mycart", jsonList,cookieOptions2);

				return View(urunsRequest);
			}

			List<CartProductsViewModel> uruns = new List<CartProductsViewModel>();
			if (urun != null)
			{
				int id=uruns.Count+1;
				urun.ID = id;
				uruns.Add(urun);
			}
			var cookieOptions = new CookieOptions()
			{
				HttpOnly = true,
				Secure = true,
				Expires = DateTime.UtcNow.AddMinutes(10)
			};

			var jsonlist=JsonConvert.SerializeObject(uruns);
			HttpContext.Response.Cookies.Append("mycart",jsonlist,cookieOptions);

			return View(uruns);

		}
	}
}
