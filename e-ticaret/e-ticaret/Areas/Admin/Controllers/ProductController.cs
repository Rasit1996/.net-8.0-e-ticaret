using DataAccessLayer.Connections;
using e_ticaret.Models;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;





namespace e_ticaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class ProductController : Controller
    {
        TContext ctx = new TContext();
        public async Task<IActionResult> Index()
        {
            var list = await ctx.Uruns.Include(x => x.category).Where(x=>x.State==true).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> add()
        {
            ViewBag.cat = await ctx.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> add(ProductViewModel model, IFormFile ImageFile)
        {
            ProductValidator proValid = new ProductValidator();
            ValidationResult result = await proValid.ValidateAsync(model);

            if (result.IsValid)
            {
                if (ImageFile == null)
                {
                    ViewBag.hata = "Lütfen bir ürün resmi seçiniz!";
                    ViewBag.cat = await ctx.Categories.ToListAsync();
                    return View(model);
                }
                string FileName = Path.ChangeExtension(ImageFile.FileName, ".jpg");
                string yol = "wwwroot";
                string path = "/Images/Product/" + FileName;
                string pathStream = yol + path;
                if (!System.IO.File.Exists(pathStream))
                {
                    var filestream = new FileStream(pathStream, FileMode.Create);
                    var image = SixLabors.ImageSharp.Image.Load(ImageFile.OpenReadStream());
                    image.Mutate(x => x.Resize(600, 750));
                    await image.SaveAsJpegAsync(filestream);
                }

                urun urun = new urun()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Stock = model.Stock,
                    Price = (double)model.Price,
                    Discount = model.Discount,
                    categoryID = (int)model.CategoryID,
                    State = true

                };
                await ctx.Uruns.AddAsync(urun);

                await ctx.SaveChangesAsync();

                urunImage urIm = new urunImage()
                {
                    urunID = urun.ID,
                    ImageUrl = path
                };

                await ctx.UrunImages.AddAsync(urIm);

                await ctx.SaveChangesAsync();

                return RedirectToAction("Index");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            ViewBag.cat = await ctx.Categories.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> delete(int id)
        {
            var urun = await ctx.Uruns.FirstOrDefaultAsync(x => x.ID == id);
            urun.State = false;
            ctx.Uruns.Update(urun);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> update(int id)
        {
            var ur = await ctx.Uruns.Include(x => x.category).FirstOrDefaultAsync(x => x.ID == id);
            TempData["id"] = ur.ID;


            ProductViewModel model = new ProductViewModel()
            {
                Name = ur.Name,
                Description = ur.Description,
                Stock = ur.Stock,
                Price = ur.Price,
                Discount = ur.Discount,
                CategoryID = ur.categoryID
            };
            ViewBag.catName = ur.category.Name;
            ViewBag.Cats = await ctx.Categories.Where(x => x.Name != ur.category.Name).ToListAsync();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> update(ProductViewModel model)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var urun = await ctx.Uruns.Include(x => x.category).FirstOrDefaultAsync(x => x.ID == id);

            ProductValidator proValid = new ProductValidator();
            ValidationResult result = await proValid.ValidateAsync(model);


            if (result.IsValid)
            {


                urun.Name = model.Name;
                urun.Description = model.Description;
                urun.Stock = model.Stock;
                urun.Price = (double)model.Price;
                urun.Discount = model.Discount;
                urun.categoryID = (int)model.CategoryID;

                ctx.Uruns.Update(urun);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            TempData["id"] = urun.ID;
            ViewBag.catName = urun.category.Name;
            ViewBag.Cats = await ctx.Categories.Where(c => c.Name != urun.Name).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Image(int id)
        {
            var urun = await ctx.Uruns.Include(u => u.urunImages).SingleOrDefaultAsync(u => u.ID == id);
            TempData["ID"] = urun.ID;
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Image(IFormFile ImageFile)
        {
            int id = Convert.ToInt32(TempData["ID"]);

            if (ImageFile == null)
            {
                ViewBag.hata = "Lütfen bir ürün resmi seçiniz!";
                ViewBag.cat = await ctx.Categories.ToListAsync();
                var urun = await ctx.Uruns.Include(u => u.urunImages).SingleOrDefaultAsync(u => u.ID == id);
                TempData["ID"] = urun.ID;
                return View();
            }
            var FileName = Path.ChangeExtension(ImageFile.FileName, ".jpg");
            string yol = "wwwroot";
            string path = "/Images/Product/" + FileName;
            string pathStream = yol + path;
            if (!System.IO.File.Exists(pathStream))
            {
                
                using (var filestream = new FileStream(pathStream, FileMode.Create))
                {

                    var image = SixLabors.ImageSharp.Image.Load(ImageFile.OpenReadStream());
                    image.Mutate(x => x.Resize(600, 750));
                    await image.SaveAsJpegAsync(filestream);
                    
                }

            }



            var urunID = ctx.Uruns.SingleOrDefault(x => x.ID == id).ID;

            urunImage urIm = new urunImage()
            {
                urunID = urunID,
                ImageUrl = path
            };
            ctx.UrunImages.Add(urIm);

            await ctx.SaveChangesAsync();
            return Ok();



        }

        [HttpPost]
        public async Task<IActionResult> ImageDelete(int id)
        {
            var urunImage = await ctx.UrunImages.FirstOrDefaultAsync(um => um.ID == id);

            ctx.UrunImages.Remove(urunImage);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> UrunFeature()
        {
            var list = await ctx.Features.Where(x=>x.state==true).GroupBy(x => x.Name).ToListAsync();

            return View(list);
        }

        public async Task<PartialViewResult> PartialFeatureName(List<UrunFeature> uf)
        {


            return PartialView(uf);
        }

        public async Task<IActionResult> FeatureName(int id)
        {
            var list = await ctx.UrunFeatures.Include(x => x.Urun).Include(x => x.Feature).Where(x => x.urunID == id).ToListAsync();
            if (list.Count == 0)
                return Json(new { error = true, message = "Bu ürüne" });

            return PartialView("PartialFeatureName", new { uf = list });
        }



        public async Task<IActionResult> FeatureAdd()
        {
            return View();
        }

        public async Task<IActionResult> FeatureDetail(string id)
        {
            var list = await ctx.Features.Where(x => x.Name == id&x.state==true).ToListAsync();
            ViewBag.isim = id;
           
            return View(list);
        }

        public IActionResult FeatureDetailAdd(string id)
        {
            ViewBag.isim = id;
            var list= ctx.Features.Where(x => x.Name == id&x.state==true).Include(x=>x.CategoryFeatures).Take(1);
           
            TempData["feaKat"] = list.First().CategoryFeatures.First().CategoryID;
            ViewBag.FeatureTitle =list.First().Title;
            
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FeatureDetailAdd(FeatureDetailViewModel model)
        {
            FeatureDetailValidator modelValid = new FeatureDetailValidator();
            int feakat = Convert.ToInt32(TempData["feaKat"]);
            ValidationResult result = modelValid.Validate(model);
            if (result.IsValid)
            {
                Feature feature = new Feature()
                {
                    Name = model.name,
                    property = model.property,
                    description = model.description,
                    Title=model.title,
                    state = true
                };
                
                ctx.Features.Add(feature);
                await ctx.SaveChangesAsync();
                CategoryFeature cf = new CategoryFeature()
                {
                    CategoryID = feakat,
                    FeatureID = feature.ID
                };
                ctx.CategoryFeatures.Add(cf);
                await ctx.SaveChangesAsync();
                return RedirectToAction("FeatureDetail", new { id = model.name });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            ViewBag.isim = model.name;
            TempData["feaKat"] = feakat;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FeatureDetailDelete(int id)
        {
            var feature = await ctx.Features.FirstOrDefaultAsync(x => x.ID == id);
            ctx.Features.Remove(feature);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> Features(int id)
        {
            var list = await ctx.UrunFeatures.Include(x => x.Feature).Where(x => x.urunID == id&x.Feature.state==true).ToListAsync();
            var urun = await ctx.Uruns.SingleOrDefaultAsync(x => x.ID == id);
            TempData["ID"] = urun.ID;
            ViewBag.isim = urun.Name;
            ViewBag.id = urun.ID;
            return View(list);
        }

        public async Task<IActionResult> UrunFeatureAdd()
        {
            int Id = Convert.ToInt32(TempData["ID"]);
            var urun = await ctx.Uruns.SingleOrDefaultAsync(x => x.ID == Id);
            ViewBag.id = urun.ID;
            var id = urun.categoryID;
            TempData["ID"] = urun.ID;

            List<IGrouping<string, CategoryFeature>> NewList = grouping(urun.ID,id);

            return View(NewList);
        }

        [HttpPost]
        public async Task<IActionResult> UrunFeatureAdd(List<int> model,int ID)
        {
            if (model.Count==0)
            {

                int catID = ctx.Uruns.SingleOrDefault(x => x.ID == ID).categoryID;

                List<IGrouping<string, CategoryFeature>> List = grouping(ID, catID);

                ViewBag.id = ID;

                ViewBag.hata = "Lütfen seçim yaptığınızdan emin olun!";

                return View(List);
            }
            foreach (var item in model)
            {
                UrunFeature uf = new UrunFeature()
                {
                    urunID = ID,
                    featureID = item
                };

                ctx.UrunFeatures.Add(uf);
                await ctx.SaveChangesAsync();
            }
           

           
            return RedirectToAction("Features", new { id = ID });

          
        }

        [HttpPost]
        public async Task<IActionResult> UrunFeatureDelete(int id)
        {
            string name = TempData["name"].ToString();
            var urunID = ctx.Uruns.SingleOrDefault(x => x.Name == name).ID;
            var urunFeature = await ctx.UrunFeatures.SingleOrDefaultAsync(x => x.urunID == urunID & x.featureID == id);

            ctx.UrunFeatures.Remove(urunFeature);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        public IActionResult FeaturesAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FeaturesAdd(FeaturesViewModel model)
        {
            FeaturesValidator validationRules = new FeaturesValidator();
            ValidationResult result = validationRules.Validate(model);
            if (result.IsValid)
            {
                Feature feature = new Feature()
                {
                    Name = model.name,
                    Title=model.title,
                    state=true, 

                };

                ctx.Features.Add(feature);
                await ctx.SaveChangesAsync();
                return RedirectToAction("UrunFeature");

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FeaturesDelete(string id)
        {
            var featureList=await ctx.Features.Where(x=>x.Name== id).ToListAsync();
            foreach (var item in featureList)
            {
                item.state = false;
                ctx.Features.Update(item);
                await ctx.SaveChangesAsync();
            }
            return Ok();
        }

        public List<IGrouping<string,CategoryFeature>> grouping(int id,int catID)
        {
            var urunFeature =  ctx.UrunFeatures.Include(x => x.Feature)
                .Where(x => x.urunID == id & x.Feature.state == true).ToList();



            List<string> urunfeaName = new List<string>();

            foreach (var item in urunFeature)
            {
                urunfeaName.Add(item.Feature.property);
            }



            var list =  ctx.CategoryFeatures.Include(x => x.Feature)
                .Where(x => x.CategoryID == catID & x.Feature.state == true & x.Feature.property != null)
                .GroupBy(x => x.Feature.Name).ToList();

            List<IGrouping<string, CategoryFeature>> NewList = new List<IGrouping<string, CategoryFeature>>();
            int state = 1;
            foreach (var group in list)
            {
                NewList.Add(group);
                var Group = NewList.FirstOrDefault(x => x.Key == group.Key);
                foreach (var item in group)
                {
                    if (urunfeaName.Contains(item.Feature.property))
                    {
                        var updateGroup = Group.Where(x => x.FeatureID != item.FeatureID).ToList();
                        if (updateGroup.Count>0)
                        {


                            Group = updateGroup.GroupBy(x => x.Feature.Name).First();
                        }
                        else
                            state = 0;
                    }

                }
                NewList.Remove(group);
                if (state == 1)
                    NewList.Add(Group);
                else
                    state = 1;


            }
            return NewList;

        }
    }
}