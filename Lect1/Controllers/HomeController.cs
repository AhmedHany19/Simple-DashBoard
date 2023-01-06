using Lect1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lect1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
           ViewBag.CategoriesCount= db.Categories.Count();
           ViewBag.PostsCount= db.Posts.Count();
           ViewBag.UsersCount= db.Users.Count();
            var latestPosts = db.Posts.Take(4).OrderByDescending(p => p.UpdateDate).ToList();
            return View(latestPosts);
        }

        public ActionResult About()
        {
          

            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }
    }
}