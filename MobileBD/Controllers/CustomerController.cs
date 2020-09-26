using MobileBD.Models;
using MobileBD.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileBD.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MobileDbContext db = new MobileDbContext();
        // GET: Customer
        [Authorize(Roles = "1,2")]
        public ActionResult CustomerTable(int PageNumber = 1)
        {
            var data = db.Customers.OrderByDescending(x => x.Membership).ToList();
            data = ApplyPaginationOnStores(data, PageNumber);

            return View(data);
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult CustomerProductList(string storeId , int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(x => x.PostDate).Where(x => x.StoreId == storeId).ToList();
            ViewBag.storeId = storeId;
            if (storeId!=null)
            {
                data = db.Products.OrderByDescending(x => x.PostDate).Where(x => x.StoreId == storeId).ToList();
                if (data.Count < 1)
                {
                    TempData["msg"] = "Please Insert Correct Store ID!!!";
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
        public ActionResult Stores(int PageNumber = 1)
        {
            var data = db.Customers.ToList();
            data = ApplyPaginationOnStores(data, PageNumber);

            return View(data);
        }
        [AllowAnonymous]
        public ActionResult StoreProducts(string id, int PageNumber = 1)
        {
            var data = db.Products.OrderByDescending(p => p.PostDate).Where(p => p.StoreId == id).ToList();
            if (data.Count < 1)
            {
                TempData["msg"] = "Sorry no Product Available!!!";
            }
            data = ApplyPagination(data, PageNumber);

            return View(data);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Create()
        {
            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            return View();
        }
        [HttpPost]
        public ActionResult Create(CustomerViewModel customervm)
        {
            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            if (customervm.UserImageFile != null)
            {
                
                    string filefName = Path.GetFileNameWithoutExtension(customervm.UserImageFile.FileName.Replace(" ", "-"));
                    string extension = Path.GetExtension(customervm.UserImageFile.FileName);

                    filefName = filefName + DateTime.Now.ToString("yymmssff") + extension;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {


                        //Save Name in DB
                        customervm.Picture = "~/CustomerImage/" + filefName;

                        //Save Image in folder
                        filefName = Path.Combine(Server.MapPath("~/CustomerImage/"), filefName);
                        customervm.UserImageFile.SaveAs(filefName);
                    }

                

            }


            Customer c = new Customer
            {
                CustomerId = customervm.CustomerId,
                FullName = customervm.FullName,
                Gender = customervm.Gender,
                StoreName = customervm.StoreName,
                StoreId = customervm.StoreId,
                Number = customervm.Number,
                Email = customervm.Email,
                Membership = customervm.Membership,
                StoreAddress = customervm.StoreAddress,
                Picture = customervm.Picture,
                OurLocationID = customervm.OurLocationID
            };
            if (ModelState.IsValid)
            {
                db.Customers.Add(c);
                db.SaveChanges();
                ViewBag.Message = "You have Registered Successfully!!!";
                return RedirectToAction("Index", "Product");

            }

            return View(customervm);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var customer = db.Customers.Find(id);
            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            Session["OldPicture"] = customer.Picture;


            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(CustomerViewModel customervm)
        {
            var DivisionList = db.OurLocations.ToList();
            ViewBag.Division = new SelectList(DivisionList, "OurLocationID", "OurLocationName");

            if (customervm.UserImageFile != null)
            {

                string filefName = Path.GetFileNameWithoutExtension(customervm.UserImageFile.FileName.Replace(" ","-"));
                string extension = Path.GetExtension(customervm.UserImageFile.FileName);

                filefName = filefName + DateTime.Now.ToString("yymmssff") + extension;

                if (customervm.Picture != "~/CustomerImage/No-img.png")
                {
                    string OldPicture = Request.MapPath(Session["OldPicture"].ToString());
                    if (customervm.Picture != filefName)
                    {
                        System.IO.File.Delete(OldPicture);
                    }
                }

                //Save Name in DB
                customervm.Picture = "~/CustomerImage/" + filefName;

                //Save Image in folder
                filefName = Path.Combine(Server.MapPath("~/CustomerImage/"), filefName);
                customervm.UserImageFile.SaveAs(filefName);
            }
            if (customervm.Picture == "/CustomerImage/No-img.png")
            {
                customervm.Picture = null;
            }
            Customer c = new Customer
            {
                CustomerId = customervm.CustomerId,
                FullName = customervm.FullName,
                Gender = customervm.Gender,
                StoreName = customervm.StoreName,
                StoreId = customervm.StoreId,
                Number = customervm.Number,
                Email = customervm.Email,
                Membership = customervm.Membership,
                StoreAddress = customervm.StoreAddress,
                Picture = customervm.Picture,
                OurLocationID = customervm.OurLocationID

            };

            db.Entry(c).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("CustomerTable");
        }
        public ActionResult Delete(int id)
        {
            var customer = db.Customers.Find(id);
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ComfirmDelete(int id)
        {
            var customer = db.Customers.Find(id);
            var picture = Request.MapPath(customer.Picture.ToString());
            if (System.IO.File.Exists(picture))
            {
                System.IO.File.Delete(picture);
            }
            db.Entry(customer).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("CustomerTable");
        }
        public List<Product> ApplyPagination(List<Product> data, int PageNumber)
        {

            ViewBag.TotalPages = Math.Ceiling(data.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            data = data.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return data;
        }
        public List<Customer> ApplyPaginationOnStores(List<Customer> data, int PageNumber)
        {

            ViewBag.TotalPages = Math.Ceiling(data.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            data = data.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return data;
        }
    }
}