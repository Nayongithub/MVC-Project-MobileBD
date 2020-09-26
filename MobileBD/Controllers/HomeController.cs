using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileBD.Models;

namespace MobileBD.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly MobileDbContext db = new MobileDbContext();
        public ActionResult Index()
        {
            return View(db.MobileBrands.ToList());
        }
        public PartialViewResult OurLocation()
        {
            var locationList = db.OurLocations.ToList();

            return PartialView("_OurLocation", locationList);

        }
        public PartialViewResult OurBrands()
        {
            var BrandList = db.MobileBrands.ToList();

            return PartialView("_OurBrands", BrandList);
        }

        public PartialViewResult PriceRange()
        {
            var data = db.Products.ToList();

            return PartialView("_PriceRange", data);
        }
        public PartialViewResult Search()
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();

            return PartialView("_Search", data);
        }
        public PartialViewResult AdvancedSearch()
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).ToList();

            return PartialView("_AdvancedSearch", data);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}