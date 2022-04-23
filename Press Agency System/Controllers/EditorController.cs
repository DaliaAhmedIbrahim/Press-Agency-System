using Press_Agency_System.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Press_Agency_System.ViewModels;


namespace Press_Agency_System.Controllers
{
    public class EditorController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public Post Posts { get; private set; }

        // GET: Editor
        public ActionResult ViewInformation(int id)
        {
            var ViewInfo = db.Users.Include(c => c.UserRole).SingleOrDefault(c => c.Id == id);
            
            if (ViewInfo == null)
                return HttpNotFound();
            return View(ViewInfo);
        }

        [HttpGet]
        public ActionResult EditInformation(int id)
        {
            var Info = db.Users.Include(c => c.UserRole).SingleOrDefault(c=> c.Id==id);
            if (Info == null)
                return HttpNotFound();

            return View(Info);
        }

        [HttpPost]
        public ActionResult EditInformation(User user)
        {
            var userInDB = db.Users.Single(m => m.Id == user.Id);

            if (userInDB == null)
                return Json(new { result = 0 });

            userInDB.UserName = user.UserName;
            userInDB.FirstName = user.FirstName;
            userInDB.LastName = user.LastName;
            userInDB.Email  = user.Email;
            userInDB.PhoneNumber = user.PhoneNumber;

            db.SaveChanges();

            return Json(new { result = 1});
        }

        [HttpGet]
        public ActionResult EditPass(int id)
        {
            var pass = db.Users.Single(m => m.Id == id);
            if (pass == null)
                return HttpNotFound();
               
            return View(pass);
        }

        [HttpPost]
        public ActionResult EditPass(User user)
        {
            var userInDB = db.Users.Single(m => m.Id == user.Id);
            if (userInDB == null)
                return Json(new { result = 0 });

            userInDB.Password = user.Password;
            userInDB.ConfirmPassword = user.ConfirmPassword;
           
            db.SaveChanges();

            return Json(new { result = 1 });

           
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

                return RedirectToAction("ViewPosts");
            }

            List<ArticleType> articleTypes = db.ArticleTypes.ToList();
            paatvm.ArticleTypes = articleTypes;
            return View("CreatePost", paatvm);
        }

        public ActionResult ViewPosts()
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

        public ActionResult ShowQuestions()
        {
            List<Post> posts = db.Posts.Where(Q=>Q.Question!= null).ToList();
            
            return View(posts);
        }

        [HttpGet]
         public ActionResult AnswerQuestion(int id)
         {
            var post = db.Posts.Single(m => m.Id == id);
            if (post == null)
                return HttpNotFound();

            return View(post);
         }


        [HttpPost]
        public ActionResult AnswerQuestion(Post post)
        {
            var postInDB = db.Posts.Single(c => c.Id == post.Id);
            if (postInDB == null)
                return Json(new { result = 0 });

            postInDB.Answer = post.Answer;
            db.SaveChanges();


            return Json(new { result = 1 });
        }


    }
}