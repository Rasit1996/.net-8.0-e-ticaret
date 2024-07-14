using DataAccessLayer.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_ticaret.ViewComponents
{
    public class HomeCategory:ViewComponent
    {
        private readonly TContext context;

        public HomeCategory(TContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await context.Categories.Where(x => x.State == true).Take(3).ToListAsync();


            return View(list);
        }
    }
}
