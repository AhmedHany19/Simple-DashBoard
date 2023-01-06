using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lect1.Models;
using System.IO;
using System.Configuration;
using PagedList;

namespace Lect1.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? page, int? PageSize)
        {
            int pageIndex = page.HasValue ? (int)page : 1;
            int defaSize = PageSize.HasValue ? (int)PageSize : 5;

            ViewBag.psize = defaSize;

            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem(){ Value = "5",Text ="5" },
                new SelectListItem(){ Value = "10",Text ="10" },
                new SelectListItem(){ Value = "15",Text ="15" },
                new SelectListItem(){ Value = "20",Text ="20" },
                new SelectListItem(){ Value = "25",Text ="25" }
            };

            var posts = db.Posts.Include(p => p.Category)
                            .OrderBy(p => p.Id)
                            .ToPagedList(pageIndex, defaSize);
            return View(posts);
        }

        public ActionResult Search(string searchFilter, int? page, int? PageSize)
        {
            IPagedList<Post> posts;

            int pageIndex = page.HasValue ? (int)page : 1;
            int defaSize = PageSize.HasValue ? (int)PageSize : 5;

            ViewBag.psize = defaSize;

            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem(){ Value = "5",Text ="5" },
                new SelectListItem(){ Value = "10",Text ="10" },
                new SelectListItem(){ Value = "15",Text ="15" },
                new SelectListItem(){ Value = "20",Text ="20" },
                new SelectListItem(){ Value = "25",Text ="25" }
            };

            if (string.IsNullOrEmpty(searchFilter))
            {
                posts = db.Posts.Include(p => p.Category)
                      .OrderBy(p => p.Id)
                      .ToPagedList(pageIndex, defaSize);
            }
            else
            {
                ViewBag.SearchFilter = searchFilter;
                posts = db.Posts.Include(p => p.Category).Where(
                       c => c.Title.ToLower().Contains(searchFilter.ToLower())
                   )
                       .OrderBy(p => p.Id)
                       .ToPagedList(pageIndex, defaSize);
            }



            return View("index", posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,CatID,Body,PostImage")] Post post)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                               ConfigurationManager.AppSettings["UserImageFolderName"].ToString());

                //Use Namespace called : System.IO and the Path class
                string fileName = Path.GetFileNameWithoutExtension(post.PostImage.FileName);

                // To Get File Extension
                string fileExtension = Path.GetExtension(post.PostImage.FileName);

                fileName = fileName.Trim()
                        + DateTime.Now.ToString("yyyyMMdd-hhmmsstt")
                        + fileExtension;

                post.ImgPath = Path.Combine(path, fileName);

                post.PostImage.SaveAs(post.ImgPath);

                post.UpdateDate = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,CatID,UpadateDate,Body,ImgPath,PostImage")] Post post)
        {
            if (ModelState.IsValid)
            {
                if (post.PostImage != null)
                {
                    if (System.IO.File.Exists(post.ImgPath))
                    {
                        System.IO.File.Delete(post.ImgPath);
                    }

                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                   ConfigurationManager.AppSettings["UserImageFolderName"].ToString());

                    //Use Namespace called : System.IO and the Path class
                    string fileName = Path.GetFileNameWithoutExtension(post.PostImage.FileName);

                    // To Get File Extension
                    string fileExtension = Path.GetExtension(post.PostImage.FileName);

                    fileName = fileName.Trim()
                            + DateTime.Now.ToString("yyyyMMdd-hhmmsstt")
                            + fileExtension;

                    post.ImgPath = Path.Combine(path, fileName);

                    post.PostImage.SaveAs(post.ImgPath);
                }

                post.UpdateDate = DateTime.Now;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Title", post.CatId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            System.IO.File.Delete(post.ImgPath);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
