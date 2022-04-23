using Press_Agency_System.Models;
using Press_Agency_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Press_Agency_System.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            var UserRoles = db.UserRoles.ToList();
            UserAndUserRolesViewModel uaurvm = new UserAndUserRolesViewModel
            {
                UserRoles = UserRoles
            };

            return View(uaurvm);
        }

        [HttpPost]
        public ActionResult AddUser(UserAndUserRolesViewModel uaurvm, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/profiles/"), pic);

                    file.SaveAs(path);

                    uaurvm.User.Image = pic;
                }

                db.Users.Add(uaurvm.User);
                db.SaveChanges();

                return Json(new { result = 1 });
            }

            return Json(new { result = 0 });
        }

        public ActionResult ViewUsers(int id)
        {
            var ViewInfo = db.Users.Include(c => c.UserRole).ToList();

            if (ViewInfo == null)
                return HttpNotFound();

            return View(ViewInfo);
        }


        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            var User = db.Users.Single(p => p.Id == id);
            db.Users.Remove(User);
            db.SaveChanges();

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            var ArticleType = db.ArticleTypes.ToList();
            PostAndArticleTypeViewModel paatvm = new PostAndArticleTypeViewModel
            {
                ArticleTypes = ArticleType,
            };
            return View(paatvm);
        }

        [HttpPost]
        public ActionResult CreatePost(PostAndArticleTypeViewModel paatvm, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/images/profiles/"), pic);

                    file.SaveAs(path);

                    paatvm.Post.Image = pic;
                }
                paatvm.Post.CreationDate = DateTime.Now;
                db.Posts.Add(paatvm.Post);
                db.SaveChanges();

                return View(ViewPosts);
            }

            List<ArticleType> articleTypes = db.ArticleTypes.ToList();
            paatvm.ArticleTypes = articleTypes;
            return View("CreatePost", paatvm);
        }

            public ActionResult ViewPosts(int id)
        {
            var ViewInfo = db.Posts.Include(c => c.ArticleType).ToList();

            if (ViewInfo == null)
                return HttpNotFound();
            return View(ViewInfo);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            var PostInfo = db.Posts.SingleOrDefault(c => c.Id == id);
            var ArticleTypes = db.ArticleTypes.ToList();

            PostAndArticleTypeViewModel paatvm = new PostAndArticleTypeViewModel
            {
                ArticleTypes = ArticleTypes,
                Post = PostInfo

            };
            return View(paatvm);
        }

        [HttpPost]
        public ActionResult EditPost(Post post)
        {
            var postInDB = db.Posts.SingleOrDefault(p => p.Id == post.Id);

            if (postInDB == null)
                return Json(new { result = 0 });

            postInDB.ArticleTitle = post.ArticleTitle;
            postInDB.ArticalBody = post.ArticalBody;
            postInDB.ArticleTypeId = post.ArticleTypeId;

            db.SaveChanges();

            return Json(new { result = 1 });
        }


        [HttpGet]
        public ActionResult DeletePost(int id)
        {
            var Post = db.Posts.Single(p => p.Id == id);
            db.Posts.Remove(Post);
            db.SaveChanges();

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewInformation(int id)
        {
            var ViewInfo = db.Users.Include(c => c.UserRole).SingleOrDefault(c => c.Id == id);

            if (ViewInfo == null)
                return HttpNotFound();
            return View(ViewInfo);
        }

        [HttpGet]
        public ActionResult RequestedPosts()
        {
            var request = db.Posts.Include( c=> c.ArticleType).Where(c=> c.Accepted== false).ToList();
                if (request == null)
                return HttpNotFound();

            return View(request);
        }

        [HttpPost]
        public ActionResult AcceptPost(int id)
        {
            var post = db.Posts.Single(p=> p.Id==id);
            if (post == null)
                return HttpNotFound();
            post.Accepted = true;
            db.SaveChanges();

            return Json(new { result = 1 });

        }

        [HttpPost]
        public ActionResult RefusePost(int id)
        {
            var Post = db.Posts.Single(p => p.Id == id);
            db.Posts.Remove(Post);
            db.SaveChanges();

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }


    }
}