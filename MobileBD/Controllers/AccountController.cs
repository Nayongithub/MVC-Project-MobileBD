using MobileBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MobileBD.Controllers
{
    public class AccountController : Controller
    {
        private readonly  MobileDbContext db = new MobileDbContext();
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var count = db.Users.Where(x => x.Name == user.Name && x.Password == user.Password).Count();
            if (count!=0)
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                return RedirectToAction("Index","Product");
            }
            else
            {
                TempData["msg"] = "User Name or Password is incorrect";
                return View();
            }
           
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Product");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserTable");
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            var customer = db.Users.Find(id);
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ComfirmDelete(int id)
        {
            var user = db.Users.Find(id);
            
            db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("UserTable");
        }
        [Authorize(Roles = "1")]
        public ActionResult UserTable(int PageNumber = 1)
        {
            var data = db.Users.ToList();
            data = ApplyPagination(data, PageNumber);

            return View(data);
        }
        public List<User> ApplyPagination(List<User> data, int PageNumber)
        {

            ViewBag.TotalPages = Math.Ceiling(data.Count() / 5.0);
            ViewBag.PageNumber = PageNumber;
            data = data.Skip((PageNumber - 1) * 5).Take(5).ToList();
            return data;
        }
    }
}