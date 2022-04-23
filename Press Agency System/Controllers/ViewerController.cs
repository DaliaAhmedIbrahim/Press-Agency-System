using Press_Agency_System.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Press_Agency_System.Controllers
{
    public class ViewerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: viewer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowAllPosts()
        {
            List<Post> posts = db.Posts.Include(c => c.ArticleType).Where(m => m.Accepted == true).ToList();

            return View(posts);
        }

        [HttpGet]
        public ActionResult Ask(int id)
        {
            var post = db.Posts.Single(m => m.Id == id);
            if (post == null)
                return HttpNotFound();

            return View(post);
        }


        [HttpPost]
        public ActionResult Ask(Post post)
        {
            var postInDB = db.Posts.Single(c => c.Id == post.Id);
            if (postInDB == null)
                return Json(new { result = 0 });

            postInDB.Question = post.Question;
            db.SaveChanges();


            return Json(new { result = 1 });
        }

        public ActionResult BeforeLogIn()
        {
            List<Post> posts = db.Posts.Include(c => c.ArticleType).ToList();

            return View(posts);
        }

        [HttpPost]
        public ActionResult LikePost(int id)
        {
            var Post = db.Posts.Single(p => p.Id == id);
            if (Post == null)
                return Json(new { result = 0 });

            Post.Likes++;
            db.SaveChanges();

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DisLikePost(int id)
        {
            var Post = db.Posts.Single(p => p.Id == id);
            if (Post == null)
                return Json(new { result = 0 });

            Post.DisLikes++;
            db.SaveChanges();

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OpenArticle(int id)
        {
            var Post = db.Posts.Include(p => p.ArticleType).Single(p => p.Id == id);
            if (Post == null)
                return Json(new { result = 0 });

            Post.ViewersNumber++;
            db.SaveChanges();

            return View(Post);
        }

        [HttpGet]
        public ActionResult SavedPosts(int id)
        {
            var SavedPosts = db.savePosts.Include(x => x.Post)
                .Include(x => x.Post.ArticleType)
                .Include(x => x.User)
                .Where(x => x.UserId == id).ToList();

            return View(SavedPosts);
        }

        [HttpPost]
        public ActionResult SavedPosts(SavePost savePost)
        {

              savePost.SaveDate = DateTime.Now;
                db.savePosts.Add(savePost);
                db.SaveChanges();
                return Json(new { result = 1 });
            


        }
    }
}