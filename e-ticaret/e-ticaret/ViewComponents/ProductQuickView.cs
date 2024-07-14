using EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;

namespace e_ticaret.ViewComponents
{
	public class ProductQuickView:ViewComponent
	{

		public async  Task<IViewComponentResult> InvokeAsync(urun u)
		{
			return View(u);
		}
	}
}
