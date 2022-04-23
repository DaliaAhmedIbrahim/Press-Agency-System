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
    
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        [HttpGet]
        public ActionResult Registration()
        {
            var UserRoles = db.UserRoles.ToList();
            UserAndUserRolesViewModel uaurvm = new UserAndUserRolesViewModel
            {
                UserRoles = UserRoles
            };

            return View(uaurvm);
        }

        [HttpPost]
        public ActionResult Registration(UserAndUserRolesViewModel uaurvm, HttpPostedFileBase file)
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
                return RedirectToAction("LogIn");
            }

            List<UserRole> department = db.UserRoles.ToList();
            uaurvm.UserRoles = department;
            return View("Registration", uaurvm);
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var email = db.Users.Include(c => c.UserRole).SingleOrDefault(u => u.Email == user.Email);

            Session["Username"] = email.UserName;
            Session["Id"] = email.Id;
            Session["Role"] = email.UserRole.Name;

            if (email == null || email.Password != user.Password)
                return View("ErrorLogin");
            if (email.UserRole.Name == "Viewer") {
                return RedirectToAction("ShowAllPosts", "Viewer", new { id = Session["Id"] });
            }
            if (email.UserRole.Name == "Editor")
            {
                return RedirectToAction("ViewInformation", "Editor", new { id = Session["Id"] });
            }
            if (email.UserRole.Name == "Admin")
            {
                return RedirectToAction("ViewInformation", "Admin", new { id = Session["Id"] });
            }

            return View("LogIn");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("LogIn", "User");
        }


    }
}