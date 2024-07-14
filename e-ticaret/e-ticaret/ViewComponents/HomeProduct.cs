using DataAccessLayer.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_ticaret.ViewComponents
{
	public class HomeProduct:ViewComponent
	{
		private readonly TContext context;

		public HomeProduct(TContext context)
		{
			this.context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var list = await context.Uruns.Include(x=>x.category).Include(x=>x.urunImages)
				.Where(x => x.State == true&x.urunImages.Count>0).ToListAsync();

			return View(list);
		}
	}
}
