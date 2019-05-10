using EnrolTraining.Common;
using EnrolTraining.Models;
using HtmlAgilityPack;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

namespace EnrolTraining.Controllers
{
    public class AccessController : Controller
    {
        Context db = new Context();
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(Company model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    db.Company.Add(model);
                    rowsAffected = db.SaveChanges();
                    int companyid = model.CompanyID;

                    EnrollSetting enrolsetting = new EnrollSetting();
                    enrolsetting.SchedulePageFormat = 1;
                    enrolsetting.ClassIDGenerationMethod = 1;
                    enrolsetting.SiteName = model.SubDomain;
                    enrolsetting.MailForm = model.Email;
                    enrolsetting.CompanyID = model.CompanyID;
                    enrolsetting.TermsCondition = "<p>Terms and Conditions</p>\r\n<ul>\r\n<li>At " + model.CompanyName + ", We do know that emergencies happen; students may reschedule registered courses for whatever reason, provided they give us 24 &ndash;hours&rsquo; prior notice before the scheduled class. We offer no refunds. Please notify us via email or phone if you&rsquo;re unable to attend. We encourage students to be early. Students who are more than 15 minutes late to scheduled class will be turned away and asked to reschedule.</li>\r\n<li>We charge $25 for the processing of any replacement card.&nbsp;</li>\r\n<li>Course books are mandatory, the American Heart Association requires that each student has a course textbook before, during, and after the course certification for reference use. Certification cards cannot be issued to students without a book present in class.</li>\r\n<li>Book prices are fixed and any promotional code applies to classroom instructions only.</li>\r\n</ul>";
                    enrolsetting.ContactUsEmail = model.Email == null ? "" : model.Email;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.EnrolSetting.Add(enrolsetting);
                    rowsAffected = db.SaveChanges();

                    model.User.CompanyID = companyid;
                    model.User.IsActive = true;
                    model.User.IsTrainingSiteAdmin = true;
                    model.User.IsWebManager = true;
                    db.User.Add(model.User);
                    rowsAffected = db.SaveChanges();
                    AddHostHeader(model.SubDomain);
                    FlashMessage.Confirmation("Account Created Successfully. Please login to access the Enrolment Management System");
                    return RedirectToAction("Login", "Access");
                }
                catch (Exception ex)
                {
                    FlashMessage.Danger(ex.Message); 
                }

            }

            return View(model);
        }

        public ActionResult Login(string returnUrl="")
        {
            //Expire Cookies
            Session["LoginUser"] = null;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult Login(SigninViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    User oUser =  db.User.Where(u => u.UserName == model.UserName).ToList().Where(u => u.Password == model.Password).FirstOrDefault();
                    //List<User> users = db.User.Where(x => x.UserName == model.UserName && x.Password == model.Password).ToList();
                    if (oUser != null) //&& users.Count > 0 && users[0].UserID > 0
                    {
                        int userid = oUser.UserID;
                        var LoginUser = (from user in db.User
                                         join company in db.Company on user.CompanyID equals company.CompanyID
                                         join enrol in db.EnrolSetting on company.CompanyID equals enrol.CompanyID
                                         where user.UserID == userid
                                         select new LoginUser {CompanyID = company.CompanyID, CompanyName = company.CompanyName, SubDomain = enrol.SiteName, EmailAddress = user.EmailAddress, FirstName =  user.FirstName, LastName = user.LastName, IsInstructor = user.IsInstructor, IsInstructorAssistant =  user.IsInstructorAssistant, IsTrainingSiteAdmin = user.IsTrainingSiteAdmin, IsWebManager = user.IsWebManager, UserID = user.UserID, ClassNumberSetting = enrol.ClassIDGenerationMethod  }).FirstOrDefault();
                        Session["LoginUser"] = LoginUser;
                        SessionWrapper.Current.CompanyID = LoginUser.CompanyID;
                        SessionWrapper.Current.UserID = LoginUser.UserID;
                        SessionWrapper.Current.SubDomain = LoginUser.SubDomain;
                        SessionWrapper.Current.CompanyName = LoginUser.CompanyName;
                        SessionWrapper.Current.ClassNumberSetting = LoginUser.ClassNumberSetting; 

                        string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain;

                        if (!System.IO.Directory.Exists(Server.MapPath(dirpath)))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(dirpath));
                        }
                        db.Configuration.ValidateOnSaveEnabled = false;
                        //User UserModel = new User { UserID = LoginUser.UserID, LastActivity = DateTime.Now };
                        User UserModel = oUser;
                        UserModel.LastActivity = DateTime.Now;
                        db.User.Attach(UserModel);
                        var entry = db.Entry(UserModel);
                        entry.Property(e => e.LastActivity).IsModified = true;
                        db.SaveChanges();


                        if (string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return RedirectToAction("UpcomingClasses", "Admin");
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        FlashMessage.Danger("User Name or Password (case sensitive) is Incorrect. ");
                    }
                    
                }
                catch (Exception ex)
                {
                    FlashMessage.Danger(ex.Message);
                }

            }

            return View(model);

        }

        public ActionResult Receipt(int id, string code)
        {
            ViewBag.id = id;
            ViewBag.code = code;
            return View();
        }


        public ActionResult IsUniqueUserName([Bind(Prefix = "User")] User USER, string UserName = "", int UserID = 0)
        {
            string username = USER == null ? UserName : USER.UserName;
            int id = USER == null ? UserID : USER.UserID;

            bool isValid = !db.User.Any(x => x.UserName == username);
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueSubDomain(string SubDomain)
        {
            bool isValid = !db.EnrolSetting.ToList().Any(x => x.SiteName == SubDomain);
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsUniqueEmail([Bind(Prefix = "User")] User USER, string EmailAddress = "" , int UserID = 0)
        {
            string email = USER == null ? EmailAddress : USER.EmailAddress;
            int id = USER == null ? UserID : USER.UserID;
            bool IsUnchangeInEditMode = db.User.Any(x => x.EmailAddress == email && x.UserID == id);
            if (IsUnchangeInEditMode)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool IsDuplicate = db.User.Any(x => x.EmailAddress == email);
                if (IsDuplicate)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet); 
                }
            }

        }

        private void AddHostHeader(string subdomain)
        {

            using (ServerManager serverManager = new ServerManager())
            {
                string hostHeader = subdomain + ".enrollmart.com";
                Site site = serverManager.Sites["hopewellcpr"];
                site.Bindings.Add("5.189.128.41:80:" + hostHeader, "http");
                site.ServerAutoStart = true;
                serverManager.CommitChanges();
            }
        }

    }
}