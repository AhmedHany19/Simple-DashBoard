using Lect1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using Microsoft.Owin.Security.Provider;
using System.Drawing.Printing;
using System.Web.UI;
using PagedList;

namespace Lect1.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categortes


        public ActionResult Index(int? page , int? PageSize)
        {
            int pageIndex = page.HasValue ? (int)page : 1;
            int defaSize = PageSize.HasValue ? (int)PageSize : 5;
            ViewBag.psize = defaSize;

            IPagedList<Category> categoriesList= db.Categories.OrderBy(p => p.Id)
                            .ToPagedList(pageIndex, defaSize);
            return View(categoriesList);
        }
        // CRUD






        public ActionResult Search(string searchFilter, int? page, int? PageSize)
        {
            int pageIndex = page.HasValue ? (int)page : 1;
            int defaSize = PageSize.HasValue ? (int)PageSize : 5;
            ViewBag.psize = defaSize;

            IPagedList<Category> categoriesList;
            if (string.IsNullOrEmpty(searchFilter))
            {
                categoriesList = db.Categories.OrderBy(p => p.Id)
                            .ToPagedList(pageIndex, defaSize);
            }
            else
                {
                ViewBag.SearchFilter = searchFilter;
                categoriesList = db.Categories.Include(p => p.Title).Where(
                       c => c.Title.ToLower().Contains(searchFilter.ToLower())
                   )
                       .OrderBy(p => p.Id)
                       .ToPagedList(pageIndex, defaSize);

            }            
            return View("Index", categoriesList);
       
}



        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Create Category";
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                db.Categories.Add(category);
                category.UpdateDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            
           
            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(Id);

            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UpdateDate = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }


            return View(category);
        }
        public ActionResult DeleteConfirmed(int? id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}