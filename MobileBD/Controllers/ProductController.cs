using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MobileBD.Models;
using MobileBD.Models.ViewModel;

namespace MobileBD.Controllers
{

    public class ProductController : Controller
    {

        private readonly MobileDbContext db = new MobileDbContext();
        

        [AllowAnonymous]
        // GET: Product
        public ActionResult Index(string searchTxt = null, int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();
            
            ViewBag.searchTxt = searchTxt;

            if (searchTxt != null)
            {
                data = db.Products.Where(p => p.MobileFullName.Contains(searchTxt) || p.Address.Contains(searchTxt)).OrderByDescending(p => p.PostDate).ToList();
                if (data.Count < 1)
                {
                    TempData["msg"] = "Sorry no match found!!!";
                }
                else
                {
                    data = ApplyPagination(data, PageNumber);
                }
                
                return View(data);
            }
            else
            {
                data = ApplyPagination(data, PageNumber);

            }
            
            return View(data);
        }
        [AllowAnonymous]
        // GET: Product
        public ActionResult AdvancedSearch(Product product)
        {
            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            var Brandlist = db.MobileBrands.ToList();
            ViewBag.Brand = new SelectList(Brandlist, "MobileBrandID", "MobileBrandName");

            return View();
        }
        [AllowAnonymous]

        public ActionResult AdvancedSearchProducts(int? OurLocationID, int? MobileBrandID, decimal maxPrice = 1000000, decimal minPrice = 1)
        {
            var MaxPrice = maxPrice + 1;
            var MinPrice = minPrice - 1;

            ViewBag.MaxPrice = MaxPrice;
            ViewBag.MinPrice = MinPrice;

            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            var Brandlist = db.MobileBrands.ToList();
            ViewBag.Brand = new SelectList(Brandlist, "MobileBrandID", "MobileBrandName");

            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();
            if (data.Count > 0)
            {
                if (OurLocationID == null & MobileBrandID == null)
                {
                    data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.Price < MaxPrice & p.Price > MinPrice).ToList();
                    
                    if (data.Count > 0)
                    {
                        
                        return View(data);
                    }
                    else
                    {
                        TempData["msg"] = "Sorry no Match Found";
                         return View(data);
                    }
                    
                }

                else if (OurLocationID==null)
                {
                    data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.MobileBrandID == MobileBrandID & p.Price < MaxPrice & p.Price > MinPrice).ToList();
                    if (data.Count > 0)
                    {

                        return View(data);
                    }
                    else
                    {
                        TempData["msg"] = "Sorry no Match Found";
                        return View(data);
                    }
                }
                else if (MobileBrandID==null)
                {
                    data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.OurLocationID == OurLocationID &  p.Price < MaxPrice & p.Price > MinPrice).ToList();
                    if (data.Count > 0)
                    {

                        return View(data);
                    }
                    else
                    {
                        TempData["msg"] = "Sorry no Match Found";
                        return View(data);
                    }
                }

                else
                {
                    data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.OurLocationID == OurLocationID & p.MobileBrandID == MobileBrandID & p.Price < MaxPrice & p.Price > MinPrice).ToList();
                    if (data.Count > 0)
                    {

                        return View(data);
                    }
                    else
                    {
                        TempData["msg"] = "Sorry no Match Found";
                        return View(data);
                    }
                }
                
            }

            return View(data);
        }
        public ActionResult ProductDetails(int? id)
        {

            var data = db.Products.Find(id);
            return View(data);

        }
        [AllowAnonymous]
        public ActionResult PriceRange(decimal maxPrice = -1, decimal minPrice = 1, int PageNumber = 1)
        {
            var MaxPrice = maxPrice + 1;
            var MinPrice = minPrice - 1;

            ViewBag.MaxPrice = MaxPrice;
            ViewBag.MinPrice = MinPrice;


            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();

            if (data.Count > 0)
            {
                
                data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.Price < MaxPrice & p.Price > MinPrice).ToList();
                if (data.Count > 0)
                {
                    data = ApplyPagination(data, PageNumber);
                    return View(data);
                }
                else
                {

                    TempData["msg"] = "Sorry, No Product is Available In Your Price Range";
                    data = ApplyPagination(data, PageNumber);
                    return View(data);

                }
            }
            
           
            return View(data);
        }

        public ActionResult ProductsByLocationId(int id, int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.OurLocationID == id).ToList();
            if (data.Count < 1)
            {

                TempData["msg"] = "Sorry no Product Available!!!";
            }
            data = ApplyPagination(data, PageNumber);
            return View(data);
        }
  
        public ActionResult ProductsByMobileBrandId(int id, int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.MobileBrandID == id).ToList();
            if (data.Count < 1)
            {
                TempData["msg"] = "Sorry no Product Available!!!";
            }
            data = ApplyPagination(data, PageNumber);
            return View(data);
        }

        [Authorize(Roles = "1,2")]
        public ActionResult ProductTable(int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();
            data = ApplyPagination(data, PageNumber);
            return View(data);
        }

        public PartialViewResult Slider()
        {
            var data = db.Products.Where(x => x.ForSlider == true).OrderByDescending(p => p.PostDate).ToList();

            return PartialView("_Slider", data);
        }
        [Authorize]
        public ActionResult Create()
        {

            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            var Brandlist = db.MobileBrands.ToList();
            ViewBag.Brand = new SelectList(Brandlist, "MobileBrandID", "MobileBrandName");

            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductViewModel productvm)
        {


            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            var Brandlist = db.MobileBrands.ToList();
            ViewBag.Brand = new SelectList(Brandlist, "MobileBrandID", "MobileBrandName");
            if (productvm.UserImageFile != null)
            {


                string filefName = Path.GetFileNameWithoutExtension(productvm.UserImageFile.FileName.Replace(" ","-"));
                string extension = Path.GetExtension(productvm.UserImageFile.FileName);

                filefName = filefName + DateTime.Now.ToString("yymmssff") + extension;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {

                    
                    //Save Name in DB
                    productvm.Picture = "~/Images/" + filefName;

                    //Save Image in folder
                    filefName = Path.Combine(Server.MapPath("~/Images/"), filefName);
                    productvm.UserImageFile.SaveAs(filefName);
                   

                }

            }

            if (productvm.Picture == "/Images/No-img.png")
            {
                productvm.Picture = null;
            }



            Product p = new Product
            {
                ProductID = productvm.ProductID,
                FullName = productvm.FullName,
                Gender = productvm.Gender,
                Number = productvm.Number,
                Email = productvm.Email,
                MobileFullName = productvm.MobileFullName,
                Condition = productvm.Condition,
                Sell = productvm.Sell,
                Exchange = productvm.Exchange,
                Price = productvm.Price,
                Address = productvm.Address,
                PostDate = productvm.PostDate,
                Description = productvm.Description,
                Picture = productvm.Picture,
                StoreId = productvm.StoreId,
                ForSlider = productvm.ForSlider,
                MobileBrandID = productvm.MobileBrandID,
                OurLocationID = productvm.OurLocationID

            };
            if (ModelState.IsValid)
            {
                db.Products.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productvm);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var product = db.Products.Find(id);

            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            var Brandlist = db.MobileBrands.ToList();
            ViewBag.Brand = new SelectList(Brandlist, "MobileBrandID", "MobileBrandName");
            Session["OldPicture"] = product.Picture;
            if (product.Picture == null)
            {
                product.Picture = "~/Images/No-img.png";
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productvm)
        {

            if (productvm.UserImageFile != null)
            {

                string filefName = Path.GetFileNameWithoutExtension(productvm.UserImageFile.FileName.Replace(" ","-"));
                string extension = Path.GetExtension(productvm.UserImageFile.FileName);

                filefName = filefName + DateTime.Now.ToString("yymmssff") + extension;

                if (productvm.Picture != "~/Images/No-img.png")
                {
                    string OldPicture = Request.MapPath(Session["OldPicture"].ToString());
                    if (productvm.Picture != filefName)
                    {
                        System.IO.File.Delete(OldPicture);
                    }
                }

                //Save Name in DB
                productvm.Picture = "~/Images/" + filefName;

                //Save Image in folder
                filefName = Path.Combine(Server.MapPath("~/Images/"), filefName);
                productvm.UserImageFile.SaveAs(filefName);
            }
            if (productvm.Picture == "~/Images/No-img.png")
            {
                productvm.Picture = null;
            }
            Product p = new Product
            {
                ProductID = productvm.ProductID,
                FullName = productvm.FullName,
                Gender = productvm.Gender,
                Number = productvm.Number,
                Email = productvm.Email,
                MobileFullName = productvm.MobileFullName,
                Condition = productvm.Condition,
                Sell = productvm.Sell,
                Exchange = productvm.Exchange,
                Price = productvm.Price,
                Address = productvm.Address,
                PostDate = productvm.PostDate,
                Description = productvm.Description,
                Picture = productvm.Picture,
                StoreId = productvm.StoreId,
                ForSlider = productvm.ForSlider,
                MobileBrandID = productvm.MobileBrandID,
                OurLocationID = productvm.OurLocationID
            };

            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ComfirmDelete(int id)
        {
            var product = db.Products.Find(id);
            var picture = Request.MapPath(product.Picture.ToString());
            if (System.IO.File.Exists(picture))
            {
                System.IO.File.Delete(picture);
            }
            db.Entry(product).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ProductTable");
        }

        public List<Product> ApplyPagination(List<Product> data, int PageNumber)
        {

            ViewBag.TotalPages = Math.Ceiling(data.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            data = data.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return data;
        }
    }
}