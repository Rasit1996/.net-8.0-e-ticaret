using DataAccessLayer.Connections;
using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
//using System.Drawing;


namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class categoryController : Controller
    {
        TContext ctx=new TContext();
        public async Task<IActionResult> Index()
        {
            var list = await ctx.Categories.ToListAsync();
            return View(list);
        }

        public IActionResult add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> add(CategoryViewModel cat, IFormFile ImageFile)
        {
            CategoryValidator catValid=new CategoryValidator();
            ValidationResult result=catValid.Validate(cat);
            if (result.IsValid)
            {
                if (ImageFile == null)
                {
                    ViewBag.hata = "Lütfen bir kategori resmi belirleyin!";
                    return View();
                }
                var newFileName = Path.ChangeExtension(ImageFile.FileName, ".jpg");
                string path = "/Images/Category/" + newFileName;
                string streamPath = "wwwroot/" + path;
                if (!System.IO.File.Exists(streamPath))
                {
                    using (var fileStream = new FileStream(streamPath, FileMode.Create))
                    {
                       
                        var image = Image.Load(ImageFile.OpenReadStream());

                        image.Mutate(x => x.Resize(600, 400));

                        image.SaveAsJpegAsync(streamPath);

                    };
                }
               

                category cate = new category()
                {
                    Name = cat.name,
                    Description = cat.description,
                    ImageUrl=path,
                    State = true
                };
                await ctx.Categories.AddAsync(cate);
                await ctx.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(cat);
        }

        public async Task<IActionResult> delete(int id)
        {
            var cat = await ctx.Categories.FirstOrDefaultAsync(x=>x.ID == id);

            ctx.Categories.Remove(cat);
            await ctx.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> update(int id)
        {
            var cat=await ctx.Categories.FirstOrDefaultAsync(x=>x.ID==id);

            var cate = new CategoryViewModel()
            {
                name = cat.Name,
                description = cat.Description
            };
            TempData["id"]=cat.ID;
            return View(cate);
        }

        [HttpPost]
        public async Task<IActionResult> update(CategoryViewModel catView, IFormFile ImageFile)
        {
            CategoryValidator catValid= new CategoryValidator();
            ValidationResult result=catValid.Validate(catView);
            if (result.IsValid)
            {


                int id = Convert.ToInt32(TempData["id"]);
                var cat= await ctx.Categories.FirstOrDefaultAsync(c => c.ID == id);

                if (ImageFile != null)
                {
                    var FileName = Path.ChangeExtension(ImageFile.FileName, ".jpg");
                  string path="/Images/Category/"+FileName;
                    string streamPath = "wwwroot/" + path;
                    if (!System.IO.File.Exists(streamPath))
                    {
                        using (FileStream fileStream = new FileStream(streamPath, FileMode.Create))
                        {
                            Image image = Image.Load(ImageFile.OpenReadStream());
                            image.Mutate(x => x.Resize(600, 400));

                            image.Save(fileStream, new JpegEncoder());
                        };
                    }

                    cat.ImageUrl = path;
                }




                cat.Name = catView.name;
                cat.Description = catView.description;

                ctx.Categories.Update(cat);

                await ctx.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(catView);
        }

        public async Task<IActionResult> FeaturesView(int id)
        {
            var list=await ctx.CategoryFeatures.Include(x=>x.Feature)
                .Where(x=>x.CategoryID==id).GroupBy(x=>x.Feature.Name).ToListAsync();

            ViewBag.isim = ctx.Categories.SingleOrDefault(x => x.ID == id).Name;
            TempData["catName"] = ViewBag.isim;
            TempData["catId"] = id;
            return View(list);
        }

        public async Task<IActionResult> FeatureAdd()
        {
            int id = Convert.ToInt32(TempData["catId"]);
            TempData["catId"] = id;

            ViewBag.id = id;

            ViewBag.isim = TempData["catName"].ToString();
            TempData["catName"] = ViewBag.isim;
            var list = FeatureName(id);
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> FeatureAdd(string name,int ID)
        {
            if (name.IsNullOrEmpty())
            {
                ViewBag.hata = "Lütfen bir özellik seçin!";

                int id = Convert.ToInt32(TempData["catId"]);
                TempData["catId"] = id;

                ViewBag.id = id;

                ViewBag.isim = TempData["catName"].ToString();
                TempData["catName"] = ViewBag.isim;
                var liste = FeatureName(id);
                return View(liste);
            }
            var list=await ctx.Features.Where(x=>x.Name == name).ToListAsync();

            foreach (var item in list)
            {
                CategoryFeature cf = new CategoryFeature()
                {
                    CategoryID = ID,
                    FeatureID = item.ID
                };
                ctx.CategoryFeatures.Add(cf);
                await ctx.SaveChangesAsync();
            }
            return RedirectToAction("FeaturesView", new {id=ID});
        }

        public  List<string> FeatureName(int id)
        {

            

            var list =  ctx.Features.Where(x=>x.state==true).GroupBy(x => x.Name).ToList();
            var catFeature =ctx.CategoryFeatures.Where(x => x.CategoryID == id).GroupBy(x => x.Feature.Name).ToList();

            List<string> feature = new List<string>();

            List<string> name = new List<string>();

            foreach (var group in catFeature)
            {
                name.Add(group.Key);
            }

            foreach (var group in list)
            {
                if (!name.Contains(group.Key))
                {
                    feature.Add(group.Key);
                }
            }
            return feature;
        }
    }
}
