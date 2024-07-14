using DataAccessLayer.Connections;
using e_ticaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace e_ticaret.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TContext context;
       
		private readonly CartController cartController;

        public HomeController(ILogger<HomeController> logger, TContext context, CartController cartController)
        {
            _logger = logger;
            this.context = context;
            this.cartController = cartController;	
        }

		public async Task<IActionResult> Index()
		{
		    List<object> data = CartControl();
			ViewData["cookie"] = data[0];
			ViewData["number"] = data[1];
			ViewData["chart"]  = data[2];
			ViewData["user"]   = data[3];
		
			return View();

		}

		public async Task<PartialViewResult> category()
        {
           

            return PartialView();
        }

        public IActionResult section()
        {
            return PartialView("section");
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
