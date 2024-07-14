using Azure.Core;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace e_ticaret.Models
{
	public class CartProductsNumber:Controller
	{
	
        public  int number()
        {

			var list = Request.Cookies["mycart"];
			ViewData["cookie"] = list;

			if (list == null)
			{
				return 0;
			}
			else
			{
				var uruns = JsonConvert.DeserializeObject<List<CartProductsViewModel>>(list);
				int number = 0;
				foreach (var item in uruns)
				{
					number += item.Number;
				}
				return number;
			}
			
		}
    }
}
