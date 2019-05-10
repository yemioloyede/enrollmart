using EnrolTraining.Common;
using EnrolTraining.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vereyon.Web;
using Novacode;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Specialized;
using System.Dynamic;

namespace EnrolTraining.Controllers
{
    [ExpireSession]
    public class AdminController : Controller
    {
        Context db = new Context();

        #region Registration Add-On
        public ActionResult AddRegistrationAdOn()
        {
            return View();
        }

        public ActionResult EditAdOn(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegistrationAdOn AdOn = db.RegistrationAdOn.Find(id);
            if (AdOn == null)
            {
                return HttpNotFound();
            }
            return View("AddRegistrationAdOn", AdOn);
        }

        [HttpPost]
        public ActionResult AddRegistrationAdOn( RegistrationAdOn model)
        {
            //[Bind(Exclude = "AdOnID")]
            //ModelState.Remove("AdOnID");
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.IsDefaultForAllRegistration == true)
                    {
                        var QueryResult = db.Database.ExecuteSqlCommand("Update RegistrationAdOns Set IsDefaultForAllRegistration = 0 Where CompanyID = " + SessionWrapper.Current.CompanyID);
                    }

                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.AdOnID > 0)
                    {
                        //db.Configuration.ValidateOnSaveEnabled = false;

                        //***** Shortcut to update all excluding some properties *****
                        //db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        //db.Entry(model).Property(x => x.Password).IsModified = false;

                        //***** update selected properties *****
                        //db.User.Attach(model);
                        //var entry = db.Entry(model);
                        //entry.Property(e => e.Email).IsModified = true;
                        // other changed properties
                        //db.SaveChanges();

                        db.RegistrationAdOn.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("RegistrationAdOns", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.RegistrationAdOn.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddRegistrationAdOn", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult RegistrationAdOns()
        {
            //ServiceReference1.Service1Client svc = new ServiceReference1.Service1Client();
            return View(db.RegistrationAdOn.Where(x=>x.CompanyID == SessionWrapper.Current.CompanyID));
        }

        public ActionResult AdOnDeliveryOptions()
        {
            return View(db.AdOnDeliveryOption.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult AdOnDeliveryOptions(AdOnDeliveryOption model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.ID > 0)
                    {
                        db.AdOnDeliveryOption.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Delivery Options Updated Successfully");
                            return RedirectToAction("RegistrationAdOns", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.AdOnDeliveryOption.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Delivery Options Updated Successfully");
                            return RedirectToAction("RegistrationAdOns", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);

        }

        public ActionResult DeleteAdOn(int id)
        {
            var model = db.RegistrationAdOn.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.RegistrationAdOn.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("RegistrationAdOns", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("RegistrationAdOns", "Admin");

        }

        #endregion

        #region Location
        public ActionResult AddLocation()
        {
            return View();
        }

        public ActionResult EditLocation(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location oLocation = db.Location.Find(id);
            if (oLocation == null)
            {
                return HttpNotFound();
            }
            return View("AddLocation", oLocation);
        }

        [HttpPost]
        public ActionResult AddLocation(Location model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.IsDefaultSelectionForClasses == true)
                    {
                        var QueryResult = db.Database.ExecuteSqlCommand("Update Locations Set IsDefaultSelectionForClasses = 0 Where CompanyID = " + SessionWrapper.Current.CompanyID);
                    }

                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.LocationID > 0)
                    {
                        db.Location.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("Locations", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.Location.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddLocation", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Locations()
        {
            return View(db.Location.Where(x=>x.CompanyID == SessionWrapper.Current.CompanyID));
        }
        #endregion


        #region Tax
        public ActionResult AddTax()
        {
            return View();
        }

        public ActionResult EditTax(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax oTax = db.Tax.Find(id);
            if (oTax == null)
            {
                return HttpNotFound();
            }
            return View("AddTax", oTax);
        }

        [HttpPost]
        public ActionResult AddTax(Tax model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.TaxID > 0)
                    {
                        db.Tax.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("Taxes", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.Tax.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddTax", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Taxes()
        {
            return View(db.Tax.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID));
        }
        #endregion


        #region Keycode Bank
        public ActionResult AddKeycodeBank()
        {
            return View();
        }

        public ActionResult EditKeycodeBank(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KeycodeBank oKeycodeBank = db.KeycodeBank.Find(id);
            if (oKeycodeBank == null)
            {
                return HttpNotFound();
            }
            return View("AddKeycodeBank", oKeycodeBank);
        }

        [HttpPost]
        public ActionResult AddKeycodeBank(KeycodeBank model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdate = model.KeycodeBankID > 0 ? true : false;
                    string keys = model.Keys;
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.KeycodeBankID > 0)
                    {                        
                        db.KeycodeBank.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                    }
                    else
                    {
                        db.KeycodeBank.Add(model);
                        rowsAffected = db.SaveChanges();
                    }
                    int keycodebankid = model.KeycodeBankID;
                    if (!string.IsNullOrWhiteSpace(keys))
                    {
                        List<Keycode> lstKeycodes = new List<Keycode>();
                        Keycode keycode = null; 
                        foreach (string key in model.Keys.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList())
                        {
                            keycode = new Keycode();
                            keycode.KeyBankID = keycodebankid;
                            keycode.DateAdded = DateTime.Now;
                            keycode.Key = key;
                            lstKeycodes.Add(keycode);
                        }

                        db.Keycode.AddRange(lstKeycodes);
                        rowsAffected = db.SaveChanges();

                        if (isUpdate)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("KeycodeBanks", "Admin");
                        }
                        else
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddKeycodeBank", "Admin");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult KeycodeBanks()
        {
            return View(db.KeycodeBank.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID));
        }

        [ChildActionOnly]
        public ActionResult Keycodes(int keycodebankid)
        {
            return View(db.Keycode.Where(x => x.KeyBankID == keycodebankid));
        }

        public ActionResult DeleteKeycodeBank(int id)
        {
            var model = db.KeycodeBank.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.KeycodeBank.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("KeycodeBanks", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("KeycodeBanks", "Admin");
        }

        public ActionResult DeleteKeycode(int id)
        {
            var model = db.Keycode.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int keybankid = model.KeyBankID;
                    int rowsAffected = 0;
                    db.Keycode.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("EditKeycodeBank", "Admin", new { id = keybankid });
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("AddKeycodeBank", "Admin");
        }

        #endregion

        #region Course Type
        public ActionResult AddCourseType()
        {
            return View();
        }

        public ActionResult EditCourseType(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseType oCourseType = db.CourseType.Find(id);
            if (oCourseType == null)
            {
                return HttpNotFound();
            }
            return View("AddCourseType", oCourseType);
        }

        [HttpPost]
        public ActionResult AddCourseType(CourseType model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.AddOnIDs != null)
                    {
                        model.AddOns = String.Join(",", model.AddOnIDs);
                    }
                    
                    if (model.CourseTypeID > 0)
                    {
                        db.CourseType.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("CourseTypes", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.CourseType.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddCourseType", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult CourseTypes()
        {
            return View(db.CourseType.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID));
        }
        #endregion

        #region File Manager Test (Not for Production)
        public ActionResult FileManager()
        {
            return View();
        }
        #endregion

        #region Promo Coe
        public ActionResult AddPromoCode()
        {
            return View();
        }

        public ActionResult EditPromoCode(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromoCode oPromoCode = db.PromoCode.Find(id);
            if (oPromoCode == null)
            {
                return HttpNotFound();
            }
            return View("AddPromoCode", oPromoCode);
        }

        [HttpPost]
        public ActionResult AddPromoCode(PromoCode model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    model.DiscountValue = Convert.ToDouble(new String(model.Discount.Where(Char.IsDigit).ToArray()));
                    if (model.CoursesAllowedArray != null)
                    {
                        model.CoursesAllowed = String.Join(",", model.CoursesAllowedArray);
                    }

                    if (model.CodeID > 0)
                    {
                        db.PromoCode.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("PromoCodes", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.PromoCode.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddPromoCode", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult PromoCodes()
        {
            return View(db.PromoCode.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID));
        }
        #endregion

        #region Certification Card Settings
        public ActionResult CardSettings()
        {
            CardSetting model = db.CardSetting.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.IsDefault == true).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult CardSettings(CardSetting model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.IsDefault == true)
                    {
                        var QueryResult = db.Database.ExecuteSqlCommand("Update CardSettings Set IsDefault = 0 Where CompanyID = " + SessionWrapper.Current.CompanyID);
                    }

                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.ProfileID > 0)
                    {
                        db.CardSetting.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        FlashMessage.Confirmation("Record Successfully Updated");
                    }
                    else
                    {
                        db.CardSetting.Add(model);
                        rowsAffected = db.SaveChanges();
                        FlashMessage.Confirmation("Record Successfully Added");
                    }
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction("CardSettings", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult GetCardProfile (int ProfileID)
        {
            CardSetting model = db.CardSetting.Find(ProfileID);

            if (Request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("CardSettings", model);
            }
        }

        public ActionResult DeleteCardSettings(int id)
        {
            var model = db.CardSetting.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.CardSetting.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("CardSettings", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("CardSettings", "Admin");

        }

        public ActionResult TestCardSetting(CardSetting card)
        {


            MemoryStream stream = new MemoryStream();
            DocX document = DocX.Create(stream);

            //CardSetting card = db.CardSetting.Find(id);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(card.TrainingCenter);
            sb.AppendLine(card.TCInfo1);
            sb.AppendLine(card.TCInfo2);
            sb.AppendLine();
            sb.AppendLine(card.CourseLocation1);
            sb.AppendLine(card.CourseLocation2);
            sb.AppendLine();
            sb.AppendLine("Instructor Name");
            string s = sb.ToString();


            for (int i=3; i>0; i-- )
            {
                string stInfo = Environment.NewLine + Environment.NewLine + "Student Name "+ i + Environment.NewLine + Environment.NewLine + "12/12/2222            12/2222";

                Table t = document.AddTable(1, 2);
                Border b = new Border();
                b.Space = 0;
                b.Color = Color.Transparent;
                t.SetBorder(TableBorderType.Bottom, b);
                t.SetBorder(TableBorderType.Top, b);
                t.SetBorder(TableBorderType.Left, b);
                t.SetBorder(TableBorderType.Right, b);
                t.SetBorder(TableBorderType.InsideH, b);
                t.SetBorder(TableBorderType.InsideV, b);

                t.SetWidths(new float[] { 500f, 400f });
                t.Alignment = Alignment.center;

                t.Design = TableDesign.TableNormal;
                t.Rows[0].Cells[0].Paragraphs.First().Append(stInfo).Font(new FontFamily("Arial")).FontSize(10);
                t.Rows[0].Cells[1].Paragraphs.First().Append(Regex.Replace(s, @"\r", "")).Font(new FontFamily("Arial")).FontSize(10);

                document.InsertTable(t);

                if (i != 1)
                {
                    document.InsertParagraph(Environment.NewLine);
                    document.InsertParagraph(Environment.NewLine);
                    document.InsertParagraph(Environment.NewLine);
                }
            }
            document.Save();

            return File(stream.ToArray(), "application/octet-stream", "CardPrint_" + card.ProfileName +".docx");
        }

        #endregion

        #region User
        public ActionResult AddUser(string view="")
        {
            ViewBag.view = view;
            return View();
        }

        public ActionResult EditUser(int id = 0, string view = "")
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User oUser = db.User.Find(id);
            if (oUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.view = view;
            return View("AddUser", oUser);
        }

        [HttpPost]
        public ActionResult AddUser(User model, string view = "")
        {
            if (model.UserName == null && model.Password == null)
            {
                ModelState.Remove("UserName");
                ModelState.Remove("Password");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.view = view;
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;

                    if (model.UserID > 0)
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.User.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        if (model.UserName == null && model.Password == null)
                        {
                            entry.Property(e => e.Password).IsModified = false;
                            entry.Property(e => e.UserName).IsModified = false;
                            entry.Property(e => e.MonitorDate).IsModified = false;
                            entry.Property(e => e.DuesPaidThru).IsModified = false;
                            entry.Property(e => e.Notes).IsModified = false;
                        }

                        if (view == "account")
                        {
                            entry.Property(e => e.IsActive).IsModified = false;
                            entry.Property(e => e.IsInstructor).IsModified = false;
                            entry.Property(e => e.IsInstructorAssistant).IsModified = false;
                            entry.Property(e => e.IsTrainingSiteAdmin).IsModified = false;
                            entry.Property(e => e.IsWebManager).IsModified = false;
                        }


                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            if (view == "instructor")
                            {
                                return RedirectToAction("EditInstructor", "Admin", new { id = model.UserID});
                            }
                            if (view == "account")
                            {
                                return RedirectToAction("UpcomingClasses", "Admin");
                            }

                            else
                            {
                                return RedirectToAction("Users", "Admin");
                            }

                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        model.isSendEmailWhenAssignedClass = true;
                        model.isSendReminderPriorClass = true;
                        db.User.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            if (view == "instructor")
                            {
                                return RedirectToAction("Instructors", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("AddUser", "Admin");
                            }
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Users(bool IsActive = true)
        {
            return View(db.User.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID && x.IsActive == IsActive));
        }
        #endregion

        #region Email Campaign
        public ActionResult AddEmailCampaign()
        {
            return View();
        }

        public ActionResult EditEmailCampaign(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailCampaign oCampaign = db.CampaignProfile.Find(id);
            if (oCampaign == null)
            {
                return HttpNotFound();
            }
            return View("AddEmailCampaign", oCampaign);
        }

        [HttpPost]
        public ActionResult AddEmailCampaign(EmailCampaign model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.ProfileID > 0)
                    {
                        db.CampaignProfile.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("EmailCampaigns", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.CampaignProfile.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddEmailCampaign", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult EmailCampaigns()
        {
            return View(db.CampaignProfile.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID));
        }
        #endregion

        #region Campaign Template
        public ActionResult AddEmail(int CampaignID=0)
        {
            ViewBag.CampaignID = CampaignID;
            return View();
        }

        public ActionResult EditEmail(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CampaignTemplate oCampaign = db.CampaignTemplate.Find(id);
            if (oCampaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = oCampaign.ProfileID;
            return View("AddEmail", oCampaign);
        }

        [HttpPost]
        public ActionResult AddEmail(CampaignTemplate model)
        {
            ViewBag.CampaignID = model.ProfileID;
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;

                    if (model.EmailID > 0)
                    {
                        db.CampaignTemplate.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("EmailCampaigns", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.CampaignTemplate.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddEmail", "Admin", new { CampaignID  = model.ProfileID});
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }
        #endregion

        #region Certificates
        public ActionResult Certificates()
        {
            List<string> lstCertificates = Common.Utilities.GetCertificates("/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/");
            return View(lstCertificates);
        }

        [HttpPost]
        public ActionResult UploadCertificate()
        {
            foreach (string file in Request.Files)
            {
                try
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf != null && !string.IsNullOrWhiteSpace(hpf.FileName))
                    {
                        string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/";
                        SaveFile(hpf, dirpath);
                        FlashMessage.Confirmation("File Uploaded Successfully");
                        return RedirectToAction("Certificates", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                        return RedirectToAction("Certificates", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                    return RedirectToAction("Certificates", "Admin");
                }
            }
            return RedirectToAction("Certificates", "Admin");
        }


        public string SaveFile(HttpPostedFileBase file, string dirpath)
        {

            //string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/";

            if (!System.IO.Directory.Exists(Server.MapPath(dirpath)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(dirpath));
            }
            // Specify the path to save the uploaded file to.
            string savePath = Server.MapPath(dirpath) + System.IO.Path.DirectorySeparatorChar;

            // Get the name of the file to upload.
            string fileName = file.FileName;

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath + fileName;


            // Check to see if a file already exists with the
            // same name as the file to upload.        
            if (System.IO.File.Exists(pathToCheck))
            {
                System.IO.File.Delete(pathToCheck);
            }

            // Append the name of the file to upload to the path.
            savePath += fileName;

            // Call the SaveAs method to save the uploaded
            // file to the specified directory.

            file.SaveAs(savePath);
            return fileName;
        }

        public ActionResult DownloadCertificate (string name)
        {
            string pathstr = "/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/" + name;
            string filepath = Server.MapPath(pathstr);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            string fileName = name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult DeleteCertificate(string name)
        {
            try
            {
                string pathstr = "/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/" + name;
                string filepath = Server.MapPath(pathstr);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    FlashMessage.Confirmation("Certificate Deleted Successfully");
                    return RedirectToAction("Certificates", "Admin");
                }
                else
                {
                    FlashMessage.Danger("Error in deleting certificate. contact your service provider");
                    return RedirectToAction("Certificates", "Admin");
                }
            }
            catch (Exception ex)
            {
                FlashMessage.Danger(ex.Message);
                return RedirectToAction("Certificates", "Admin");

            }

        }
        #endregion

        #region Site Settings
        public ActionResult SiteSetting()
        {
            Company company = db.Company.Find(SessionWrapper.Current.CompanyID);
            IEnumerable<Discipline> disciplines = db.Discipline.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);
            EnrollSetting enrolsetting = db.EnrolSetting.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID).FirstOrDefault();
            IEnumerable<EnrollQuestion> questions = db.EnrolQuestion.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);
            SiteSetting sitesetting = new Models.SiteSetting{ Company = company, Disciplines = disciplines, EnrolSetting = enrolsetting, EnrolQuestions = questions};

            return View(sitesetting);
        }

        [HttpPost]
        public ActionResult UpdateCompanySetting([Bind(Prefix = "Company")] Company model)
        {
            ModelState.Remove("Company.SubDomain");
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Company.Attach(model);
                    var entry = db.Entry(model);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Updated");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateEnrollSetting([Bind(Prefix = "EnrolSetting")] EnrollSetting model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    db.EnrolSetting.Attach(model);
                    var entry = db.Entry(model);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    db.Entry(model).Property(x => x.SiteName).IsModified = false;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Updated");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult AddCustomDiscipline(SiteSetting model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.Discipline.CompanyID = SessionWrapper.Current.CompanyID;
                    db.Discipline.Add(model.Discipline);
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Added");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                    return RedirectToAction("SiteSetting", "Admin");
                }

            }
            return View(model);

        }


        public ActionResult DeleteDiscipline(int id)
        {
            var model = db.Discipline.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.Discipline.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("SiteSetting", "Admin");
        }

        public ActionResult AddEnrollQuestion()
        {
            return View();
        }

        public ActionResult EditEnrollQuestion(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollQuestion oQuestion = db.EnrolQuestion.Find(id);
            if (oQuestion == null)
            {
                return HttpNotFound();
            }
            return View("AddEnrollQuestion", oQuestion);
        }

        [HttpPost]
        public ActionResult AddEnrollQuestion(EnrollQuestion model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    if (model.QuestionID > 0)
                    {
                        db.EnrolQuestion.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("SiteSetting", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.EnrolQuestion.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddEnrollQuestion", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }


        public ActionResult DeleteEnrollQuestion(int id)
        {
            var model = db.EnrolQuestion.Find(id); //returns a single item.

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.EnrolQuestion.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("SiteSetting", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("SiteSetting", "Admin");


        }

        #endregion

        #region Keep Session Alive
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }
        #endregion

        public JsonResult GetSubDomain ()
        {
            return Json(SessionWrapper.Current.SubDomain, JsonRequestBehavior.AllowGet);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult CoursesAllowed(string CoursesAllowed)
        {
            IEnumerable<CourseType> coursetypes = db.CourseType.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);
            ViewBag.CoursesAllowed = CoursesAllowed;
            return PartialView("_CoursesAllowed", coursetypes);
        }

        public ActionResult SendEmail(string email, string subject, string body, int campid)
        {
            try
            {

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("irfanibrahim1984@gmail.com", "###");
                    EmailCampaign campaign = db.CampaignProfile.Find(campid);

                    MailAddress from = new MailAddress(campaign.MailFrom, campaign.MailFrom);
                    MailAddress to = new MailAddress(email);

                    MailMessage mail = new MailMessage(from,to);
                    //mail.Sender = new MailAddress(campaign.MailFrom);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.ReplyToList.Add(campaign.MailFrom);

                    client.Send(mail);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        #region Class Scheduling

        public ActionResult ScheduleClass()
        {
            return View();
        }


        public ActionResult EditClass(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleClass oClass = null;
            if (id > 0)
            {
                oClass = (from cls in db.ScheduleClass
                          join course in db.CourseType on cls.CourseID equals course.CourseTypeID
                          where cls.ClassID == id && cls.CompanyID == SessionWrapper.Current.CompanyID
                          select new { ClassDuration = cls.ClassDuration, ClassID = cls.ClassID, ClassNo = cls.ClassNo, ClientID = cls.ClientID, CompanyID = cls.CompanyID, CourseID = cls.CourseID, CourseName = course.CourseName, EndTime = cls.EndTime, IncludeOnline = cls.IncludeOnline, InstructorID = cls.InstructorID, AssistantID = cls.AssistantID, InternalNotes = cls.InternalNotes, LocationID = cls.LocationID, MaxStudents = cls.MaxStudents, Price = cls.Price, PublicNotes = cls.PublicNotes, ScheduleDate = cls.ScheduleDate, StartTime = cls.StartTime, StudentManikinRatio = cls.StudentManikinRatio})
                            .AsEnumerable().Select(x => new ScheduleClass { ClassDuration = x.ClassDuration, ClassID = x.ClassID, ClassNo = x.ClassNo, ClientID = x.ClientID, CompanyID = x.CompanyID, CourseID = x.CourseID, CourseName = x.CourseName, EndTime = x.EndTime, IncludeOnline = x.IncludeOnline, InstructorID = x.InstructorID, AssistantID = x.AssistantID, InternalNotes = x.InternalNotes, LocationID = x.LocationID, MaxStudents = x.MaxStudents, Price = x.Price, PublicNotes = x.PublicNotes, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, StudentManikinRatio = x.StudentManikinRatio}).FirstOrDefault();
            }

            if (oClass == null)
            {
                return HttpNotFound();
            }
            List<ClassTime> lstTimes = db.ClassTime.Where(x => x.ClassID == id).ToList();

            oClass.ClassDate = new DateTime[lstTimes.Count+1];
            oClass.TimeFrom = new string[lstTimes.Count+1];
            oClass.TimeTo = new string[lstTimes.Count+1];
            oClass.ClassDate[0] = oClass.ScheduleDate;
            oClass.TimeFrom[0] = oClass.StartTime;
            oClass.TimeTo[0] = oClass.EndTime;
            for (int i=0; i < lstTimes.Count; i++)
            {
                oClass.ClassDate[i+1] = lstTimes[i].ClassDate;
                oClass.TimeFrom[i+1] = lstTimes[i].TimeFrom;
                oClass.TimeTo[i+1] = lstTimes[i].TimeTo;
            }

            return View("ScheduleClass", oClass);
        }

        [HttpPost]
        public ActionResult ScheduleClass(ScheduleClass model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    model.ScheduleDate = model.ClassDate[0];
                    model.StartTime = model.TimeFrom[0];
                    model.EndTime = model.TimeTo[0];

                    if (model.ClassDate.Length > 1)
                    {
                        int number = model.ClassDate.Length;
                        string extracls = "This class can also be meet on ";
                        for (int i = 1; i < number; i++)
                        {
                            if (i == number-1)
                            {
                                extracls = extracls + model.ClassDate[i].ToString("ddd M/d/yyyy") + " from " + model.TimeFrom[i] + " to " + model.TimeTo[i] ;
                            }
                            else
                            {
                                extracls = extracls + model.ClassDate[i].ToString("ddd M/d/yyyy") + " from " + model.TimeFrom[i] + " to " + model.TimeTo[i] + " and ";
                            }
                        }

                        model.ExtraClassTimes = extracls;
                    }

                    if (model.ClassID > 0)
                    {
                        db.ScheduleClass.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        if (SessionWrapper.Current.ClassNumberSetting == 1 || SessionWrapper.Current.ClassNumberSetting == 2) // Auto Assigned
                        {
                            entry.Property(e => e.ClassNo).IsModified = false;
                        }

                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            UpdateClassTimes(model);
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("UpcomingClasses", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                    else
                    {
                        if (SessionWrapper.Current.ClassNumberSetting == 1) // Auto Assigned
                        {
                            model.ClassNo = db.ScheduleClass.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID).Max(x => x.ClassNo).GetValueOrDefault() + 1;
                        }
                            
                        db.ScheduleClass.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            UpdateClassTimes(model);
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("EditClass", "Admin", new { id = model.ClassID});
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        private void UpdateClassTimes(ScheduleClass model)
        {
            
            var QueryResult = db.Database.ExecuteSqlCommand("Delete from ClassTimes Where ClassID = " + model.ClassID);
            List<ClassTime> lstTimes = new List<ClassTime>();
            ClassTime oTime = null;
            int number = model.ClassDate.Length;
            for (int i = 1; i < number; i++)
            {
                oTime = new ClassTime();
                oTime.ClassID = model.ClassID;
                oTime.ClassDate = model.ClassDate[i];
                oTime.TimeFrom = model.TimeFrom[i];
                oTime.TimeTo = model.TimeTo[i];
                lstTimes.Add(oTime);
            }
            db.ClassTime.AddRange(lstTimes);
            db.SaveChanges();
        }

        public ActionResult UpcomingClasses()
        {
            //List<ClassesViewModel> classes = db.Database.SqlQuery<ClassesViewModel>("").ToList();

            //Inner and Left Join Example
            //var query = from s in db.staff
            //            join m in db.manager on s.manager_id equals m.id
            //            join t in db.training on s.id equals t.staff_id into tr
            //            from training in tr.DefaultIfEmpty()
            //            select new
            //            {
            //                Staff = s,
            //                Training = training
            //            };

            //IEnumerable<ClassesViewModel> classes = (from schedule in db.ScheduleClass
            //               join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
            //               where schedule.CompanyID == SessionWrapper.Current.CompanyID && schedule.ScheduleDate >= DateTime.Today
            //               select new ClassesViewModel { ClassID = schedule.ClassID, ClassNo = schedule.ClassNo, Course = course.CourseName, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, MaxStudents = schedule.MaxStudents, Instructor = "", RegisteredStudents = db.Student.Where(x=> x.ClassID == schedule.ClassID && x.IsUnSchedule == false).Count()  });

            return View();
        }



        [HttpPost]
        public ActionResult UpcomingClasses(ClassFiltersViewModel model)
        {
            return View(model);
        }


        [ChildActionOnly]
        public ActionResult RegisteredStudents(int ClassID)
        {
            ViewBag.ClassID = ClassID;
            return View(db.Student.Where(x => x.ClassID == ClassID && x.IsUnSchedule == false));
        }


        [HttpPost]
        public ActionResult UploadClassDocuments(int ClassID)
        {
            foreach (string file in Request.Files)
            {
                try
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf != null && !string.IsNullOrWhiteSpace(hpf.FileName))
                    {
                        string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/classdocs/" + ClassID + "/";
                        SaveFile(hpf, dirpath);
                        return Json(1, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception ex)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassDocs(int ClassID)
        {
            string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/classdocs/" + ClassID + "/";
            List<string> lstCertificates = Common.Utilities.GetCertificates(dirpath);
            return PartialView(lstCertificates);
        }

        public ActionResult DeleteClassDocs (int ClassID, string FileName)
        {
            try
            {
                string pathstr = "/uploads/" + SessionWrapper.Current.SubDomain + "/classdocs/" + ClassID + "/" + FileName;
                string filepath = Server.MapPath(pathstr);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    return RedirectToAction("GetClassDocs", "Admin", new { ClassID = ClassID });
                }
                else
                {
                    return Content("File Not Exist");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }


        public ActionResult PastClasses()
        {
            //IEnumerable<ClassesViewModel> classes = (from schedule in db.ScheduleClass
            //                                         join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
            //                                         where schedule.CompanyID == SessionWrapper.Current.CompanyID && schedule.ScheduleDate < DateTime.Today
            //                                         select new ClassesViewModel { ClassID = schedule.ClassID, ClassNo = schedule.ClassNo, Course = course.CourseName, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, MaxStudents = schedule.MaxStudents, Instructor = "", RegisteredStudents = db.Student.Where(x => x.ClassID == schedule.ClassID && x.IsUnSchedule == false).Count() });
            return View();
        }

        [HttpPost]
        public ActionResult PastClasses(ClassFiltersViewModel model)
        {
            //IEnumerable<ClassesViewModel> classes = (from schedule in db.ScheduleClass
            //                                         join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
            //                                         where schedule.CompanyID == SessionWrapper.Current.CompanyID && schedule.ScheduleDate < DateTime.Today
            //                                         select new ClassesViewModel { ClassID = schedule.ClassID, ClassNo = schedule.ClassNo, Course = course.CourseName, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, MaxStudents = schedule.MaxStudents, Instructor = "", RegisteredStudents = db.Student.Where(x => x.ClassID == schedule.ClassID && x.IsUnSchedule == false).Count() });
            return View(model);
        }

        public ActionResult AddStudent(int id)
        {
            Student student = (from schedule in db.ScheduleClass
                               join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                               where schedule.ClassID == id
                               select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, CourseType = course.Type, isPromptCertification = course.CourseOptions_PromptForCertificationDuringRegistration, KeycodeBankID = course.KeycodeBankID, ShippingCost = course.ShippingPrice, locationID = schedule.LocationID})
                              .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, CourseType = x.CourseType, IsPromptCertification = x.isPromptCertification, KeycodeBankID = x.KeycodeBankID, ShippingCostMaterial = x.ShippingCost, LocationID = x.locationID}).FirstOrDefault();
            return View(student);
        }


        public ActionResult EditStudent(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student oStudent = (from schedule in db.ScheduleClass
                                join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                join student in db.Student on schedule.ClassID equals student.ClassID
                                where student.StudentID == id
                                select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = student.ClassPrice, optionsPrice = student.OptionsPrice, shippingPrice = student.ShippingPrice, totalPrice = student.TotalClassPrice, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID > 0? student.ClientID : schedule.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, CourseType = course.Type, IsPromptCertification = course.CourseOptions_PromptForCertificationDuringRegistration, KeyCodeID = student.KeycodeID, CertificationType =student.CertificationType, IsunScheduled = student.IsUnSchedule, KeycodeBankID = course.KeycodeBankID, ShippingCost = course.ShippingPrice, locationID = schedule.LocationID, keycode = student.Keycode, PromoCodeID = student.PromoCodeID, ReceiptCode = student.ReceiptCode, paymentsTotal = db.StudentPayment.Where(x=> x.StudentID == id).Sum(x => (int?)x.Amount) ?? 0 })
                  .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, OptionsPrice = x.optionsPrice, ShippingPrice = x.shippingPrice, TotalClassPrice = x.totalPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, CourseType = x.CourseType, IsPromptCertification = x.IsPromptCertification, KeycodeID = x.KeyCodeID, CertificationType = x.CertificationType, IsUnSchedule = x.IsunScheduled, KeycodeBankID = x.KeycodeBankID, ShippingCostMaterial = x.ShippingCost, LocationID = x.locationID, Keycode = x.keycode, PromoCodeID = x.PromoCodeID, ReceiptCode = x.ReceiptCode, PaymentsTotal = x.paymentsTotal }).FirstOrDefault();
            if (oStudent == null)
            {
                return HttpNotFound();
            }
            return View("AddStudent", oStudent);

        }



        public ActionResult ResendEmail(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student oStudent = (from schedule in db.ScheduleClass
                                join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                join student in db.Student on schedule.ClassID equals student.ClassID
                                where student.StudentID == id
                                select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID > 0 ? student.ClientID : schedule.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, CourseType = course.Type, IsPromptCertification = course.CourseOptions_PromptForCertificationDuringRegistration, KeyCodeID = student.KeycodeID, CertificationType = student.CertificationType, IsunScheduled = student.IsUnSchedule, KeycodeBankID = course.KeycodeBankID, ShippingCost = course.ShippingPrice, locationID = student.LocationID})
                  .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName,  ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, CourseType = x.CourseType, IsPromptCertification = x.IsPromptCertification, KeycodeID = x.KeyCodeID, CertificationType = x.CertificationType, IsUnSchedule = x.IsunScheduled, KeycodeBankID = x.KeycodeBankID, ShippingCostMaterial = x.ShippingCost, LocationID = x.locationID}).FirstOrDefault();

            if (oStudent == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrWhiteSpace(oStudent.Email))
            {
                FlashMessage.Danger("No Email Address found");
            }
            else
            {
                Common.Utilities.SendClassRegistrationConfirmationToStudent(oStudent);
                FlashMessage.Confirmation("Email sent successfully");
            }
            return RedirectToAction("EditClass", "Admin", new { id = oStudent.ClassID });

        }

        [HttpPost]
        public ActionResult AddStudent(Student model)
        {
            ModelState.Remove("CardNo");
            ModelState.Remove("ExpirationMonth");
            ModelState.Remove("ExpirationYear");
            ModelState.Remove("SecurityCode");
            ModelState.Remove("PaymentType");
            ModelState.Remove("TermsAndConditionAgreed");

            ModelState.Remove("PrimaryPhone");
            ModelState.Remove("MailingAddress1");
            ModelState.Remove("MailingCity");
            ModelState.Remove("MailingState");
            ModelState.Remove("MailingZip");
            ModelState.Remove("PromoCodeID");

            if (ModelState.IsValid)
            {
                try
                {
                    model.CompanyID = SessionWrapper.Current.CompanyID;

                    if (model.SelectedAddOns != null && model.SelectedAddOns.Length > 0)
                    {

                        model.SelectedOptions = String.Join(",", model.SelectedAddOns);
                        int[] addons = Array.ConvertAll(model.SelectedOptions.Split(','), int.Parse);
                        List<RegistrationAdOn> lstAddOns = db.RegistrationAdOn.Where(x => addons.Contains(x.AdOnID)).ToList();
                        model.OptionsPrice = lstAddOns.Select(x => x.Price).Sum();
                    }

                    if (model.DeliveryOption == "Ship")
                    {
                        model.ShippingPrice = model.ShippingCostMaterial??0;

                    }
                    else
                    {
                        model.ShippingPrice = 0;
                    }
                    model.TotalClassPrice = model.ClassPrice + model.OptionsPrice + model.ShippingPrice;

                    if (model.PromoCodeID < 1)
                    {
                        model.PromoCode = "";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(model.PromoCode))
                        {
                            model.PromoCode = db.PromoCode.Find(model.PromoCodeID).Code;
                        }
                    }

                    int rowsAffected = 0;
                    if (model.StudentID > 0)
                    {

                        if (!string.IsNullOrWhiteSpace(model.RescheduleClassID))
                        {
                            if (model.RescheduleClassID == "n")
                            {
                                //create new class for the student
                            }
                            else if (model.RescheduleClassID == "u")
                            {
                                //unschedule the student from class
                                model.IsUnSchedule = true;
                            }
                            else
                            {
                                //assign thee student to selected class
                                model.ClassID = Convert.ToInt32(model.RescheduleClassID);
                            }

                        }

                        model.PrimaryPhone = string.IsNullOrWhiteSpace(model.PrimaryPhone) ? "" : model.PrimaryPhone;
                        model.MailingAddress1 = string.IsNullOrWhiteSpace(model.MailingAddress1) ? "" : model.MailingAddress1;
                        model.MailingCity = string.IsNullOrWhiteSpace(model.MailingCity) ? "" : model.MailingCity;
                        model.MailingState = string.IsNullOrWhiteSpace(model.MailingState) ? "" : model.MailingState;
                        model.MailingZip = string.IsNullOrWhiteSpace(model.MailingZip) ? "" : model.MailingZip;

                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.Student.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;

                        entry.Property(e => e.PaymentType).IsModified = false;
                        entry.Property(e => e.KeycodeID).IsModified = false;
                        entry.Property(e => e.Keycode).IsModified = false;
                        entry.Property(e => e.RegisterionDate).IsModified = false;
                        entry.Property(e => e.QuestionIDs).IsModified = false;
                        entry.Property(e => e.Answers).IsModified = false;
                        entry.Property(e => e.TermsAndConditionAgreed).IsModified = false;


                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            Common.Utilities.UpdadteCourseAdons(model.SelectedOptions, model.StudentID);
                            Common.Utilities.ApplyPromoCode(model.StudentID, model.PromoCodeID, model.ClassPrice, model.TotalClassPrice);

                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("EditStudent", "Admin", new { id = model.StudentID });
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {



                        model.RegisterionDate = DateTime.Now;
                        model.ReceiptCode = DateTime.Now.ToString("hhmmssMMyydd");
                        model.PrimaryPhone = string.IsNullOrWhiteSpace(model.PrimaryPhone) ? "" : model.PrimaryPhone;
                        model.MailingAddress1 = string.IsNullOrWhiteSpace(model.MailingAddress1) ? "" : model.MailingAddress1;
                        model.MailingCity = string.IsNullOrWhiteSpace(model.MailingCity) ? "" : model.MailingCity;
                        model.MailingState = string.IsNullOrWhiteSpace(model.MailingState) ? "" : model.MailingState;
                        model.MailingZip = string.IsNullOrWhiteSpace(model.MailingZip) ? "" : model.MailingZip;

                        if (model.KeycodeBankID > 0)
                        {
                            Keycode keycode = Common.Utilities.RegisterKeycode(Convert.ToInt32(model.KeycodeBankID), model.FirstName, model.LastName, model.Email, model.ClassID);
                            if (keycode != null && keycode.KeycodeID > 0)
                            {
                                model.KeycodeID = keycode.KeycodeID;
                                model.Keycode = keycode.Key;
                            }
                        }


                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.Student.Add(model);
                        rowsAffected = db.SaveChanges();



                        if (rowsAffected > 0)
                        {
                            Common.Utilities.AssignKeycodesToCourseAddons(model.SelectedOptions, model.FirstName, model.LastName, model.Email, model.ClassID, model.StudentID);
                            Common.Utilities.ApplyPromoCode(model.StudentID, model.PromoCodeID, model.ClassPrice, model.TotalClassPrice);
                            Common.Utilities.SendClassRegistrationConfirmationToStudent(model);

                            FlashMessage.Confirmation("Student Add Successfully to this class");
                            return RedirectToAction("EditStudent", "Admin", new { id = model.StudentID});
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);

        }


        public ActionResult AssignKeycodeToCourse(int KeyBankID, int StudentID)
        {
            if (KeyBankID > 0)
            {
                var student = db.Student.Find(StudentID);
                Keycode keycode = Common.Utilities.RegisterKeycode(KeyBankID, student.FirstName, student.LastName, student.Email, student.ClassID);
                if (keycode != null && keycode.KeycodeID > 0)
                {
                    student.KeycodeID = keycode.KeycodeID;
                    student.Keycode = keycode.Key;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Student.Attach(student);
                    var entry = db.Entry(student);
                    entry.Property(e => e.Keycode).IsModified = true;
                    entry.Property(e => e.KeycodeID).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    FlashMessage.Warning("The associated keybank has no unused key");
                }
            }
            return RedirectToAction("EditStudent", "Admin", new { id = StudentID });
        }

        public ActionResult AssignKeycodeToAddOn(int AddOnID, int StudentID)
        {
            int? KeyBankID = db.RegistrationAdOn.Find(AddOnID).KeycodeID;

            if (KeyBankID > 0)
            {
                var student = db.Student.Find(StudentID);
                Keycode keycode = Common.Utilities.RegisterKeycode( Convert.ToInt32(KeyBankID), student.FirstName, student.LastName, student.Email, student.ClassID);
                if (keycode != null && keycode.KeycodeID > 0)
                {
                    string instructions = db.KeycodeBank.Find(keycode.KeyBankID).Instructions;


                    db.Configuration.ValidateOnSaveEnabled = false;
                    var createdAdon = db.CourseAdon.Where(x=> x.AdonID == AddOnID && x.StudentID == StudentID).FirstOrDefault();
                    createdAdon.Keycode = keycode.Key;
                    createdAdon.KeycodeID = keycode.KeycodeID;
                    createdAdon.KeycodeInstructions = instructions;
                    db.CourseAdon.Attach(createdAdon);
                    var entry = db.Entry(createdAdon);

                    entry.Property(e => e.Keycode).IsModified = true;
                    entry.Property(e => e.KeycodeID).IsModified = true;
                    entry.Property(e => e.KeycodeInstructions).IsModified = true;

                    db.SaveChanges();
                }
                else
                {
                    FlashMessage.Warning("The associated keybank has no unused key");
                }

            }
            return RedirectToAction("EditStudent", "Admin", new { id = StudentID });
        }
        [HttpPost]
        public ActionResult QuickAddStudent(Student model)
        {
            ModelState.Remove("CardNo");
            ModelState.Remove("ExpirationMonth");
            ModelState.Remove("ExpirationYear");
            ModelState.Remove("SecurityCode");
            ModelState.Remove("PaymentType");
            ModelState.Remove("TermsAndConditionAgreed");

            ModelState.Remove("Email");
            ModelState.Remove("PrimaryPhone");
            ModelState.Remove("MailingAddress1");
            ModelState.Remove("MailingCity");
            ModelState.Remove("MailingState");
            ModelState.Remove("MailingZip");

            if (ModelState.IsValid)
            {
                try
                {

                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;
                    model.RegisterionDate = DateTime.Now;
                    model.ReceiptCode = DateTime.Now.ToString("hhmmssMMyydd");
                    model.Email = string.IsNullOrWhiteSpace(model.Email) ? "" : model.Email;
                    model.PrimaryPhone = string.IsNullOrWhiteSpace(model.PrimaryPhone) ? "" : model.PrimaryPhone;
                    model.MailingAddress1 = string.IsNullOrWhiteSpace(model.MailingAddress1) ? "" : model.MailingAddress1;
                    model.MailingCity = string.IsNullOrWhiteSpace(model.MailingCity) ? "" : model.MailingCity;
                    model.MailingState = string.IsNullOrWhiteSpace(model.MailingState) ? "" : model.MailingState;
                    model.MailingZip = string.IsNullOrWhiteSpace(model.MailingZip) ? "" : model.MailingZip;

                    if (model.KeycodeBankID > 0)
                    {
                        Keycode keycode = Common.Utilities.RegisterKeycode(Convert.ToInt32(model.KeycodeBankID), model.FirstName, model.LastName, model.Email, model.ClassID);
                        if (keycode != null && keycode.KeycodeID > 0)
                        {
                            model.KeycodeID = keycode.KeycodeID;
                            model.Keycode = keycode.Key;
                        }
                    }


                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Student.Add(model);
                    rowsAffected = db.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        Common.Utilities.AssignKeycodesToCourseAddons(model.SelectedOptions, model.FirstName, model.LastName, model.Email, model.ClassID, model.StudentID);
                        if (!string.IsNullOrWhiteSpace(model.Email))
                        {
                            Common.Utilities.SendClassRegistrationConfirmationToStudent(model);
                        }

                        FlashMessage.Confirmation("Student Add Successfully to this class");
                        return RedirectToAction("EditStudent", "Admin", new { id = model.StudentID});
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);


        }

        public ActionResult StudentSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentSearch(StudentSearchViewModel model)
        {
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ApplyStudentSearch(string Filter, string Search, string LastName)
        {
            IEnumerable<Student> students = null;
            if (Filter == "Name")
            {
                students = (from schedule in db.ScheduleClass
                                    join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                    join student in db.Student on schedule.ClassID equals student.ClassID
                                    where (student.FirstName.Contains(Search) || student.LastName.Contains(LastName) ) && schedule.CompanyID == SessionWrapper.Current.CompanyID
                                    select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
                                    .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate });
            }
            else if (Filter == "Email")
            {
                students = (from schedule in db.ScheduleClass
                            join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                            join student in db.Student on schedule.ClassID equals student.ClassID
                            where student.Email.Contains(Search) && schedule.CompanyID == SessionWrapper.Current.CompanyID
                            select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
                                    .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate });
            }

            else if (Filter == "Phone")
            {
                students = (from schedule in db.ScheduleClass
                            join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                            join student in db.Student on schedule.ClassID equals student.ClassID
                            where student.PrimaryPhone.Contains(Search) && schedule.CompanyID == SessionWrapper.Current.CompanyID
                            select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
                                    .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate });
            }
            return View(students);
        }

        public ActionResult UnscheduledStudents()
        {
           IEnumerable<Student> students = (from schedule in db.ScheduleClass
                        join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                        join student in db.Student on schedule.ClassID equals student.ClassID
                        where student.IsUnSchedule == true && schedule.CompanyID == SessionWrapper.Current.CompanyID
                                            select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
                    .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate });
            return View(students);
        }

        public ActionResult EditScores(int id)
        {
            Student[] students = (from schedule in db.ScheduleClass
                                             join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                             join student in db.Student on schedule.ClassID equals student.ClassID
                                             where schedule.ClassID == id && student.IsUnSchedule == false && schedule.CompanyID == SessionWrapper.Current.CompanyID
                                             select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
         .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate }).ToArray();

            ScoresViewModel scores = new ScoresViewModel();
            if (students!= null && students.Length > 0)
            {
                scores.ClassID = students[0].ClassID;
                scores.CourseName = students[0].CourseName;
                scores.ScheduleDate = students[0].ScheduleDate;
                scores.StartTime = students[0].StartTime;
                scores.EndTime = students[0].EndTime;

                scores.StudentID = new int[students.Length];
                scores.FirstName = new string[students.Length];
                scores.LastName = new string[students.Length];
                scores.Status = new int?[students.Length];
                scores.Score = new string[students.Length];
                for (int i= 0; i< students.Length; i++)
                {
                    scores.StudentID[i] = students[i].StudentID;
                    scores.FirstName[i] = students[i].FirstName;
                    scores.LastName[i] = students[i].LastName;
                    scores.Status[i] = students[i].StatusID;
                    scores.Score[i] = students[i].TestScore;
                }
            }

            return View(scores);
        }

        [HttpPost]
        public ActionResult EditScores(ScoresViewModel model)
        {
            for(int i=0; i<model.StudentID.Length; i++)
            {
                string query = "Update Students Set TestScore='" +  model.Score[i] + "', StatusID='" + model.Status[i] + "' where StudentID=" + model.StudentID[i];
                db.Database.ExecuteSqlCommand(query);
            }
            FlashMessage.Confirmation("Record Successfully Updated");
            return RedirectToAction("EditClass", "Admin", new { id = model.ClassID });
        }

        [OutputCache(NoStore = true, Duration =0)]
        public ActionResult StudentList(int id)
        {
            List<Student> students = (from schedule in db.ScheduleClass
                                             join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                             join student in db.Student on schedule.ClassID equals student.ClassID
                                             where student.IsUnSchedule == false && schedule.ClassID == id && schedule.CompanyID == SessionWrapper.Current.CompanyID
                                             select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate })
                                             .AsEnumerable().Select(x => new Student { ClassID = x.ClassID, CourseName = x.CourseName, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, CourseAddOns = x.CourseAddOns, AlternatePhone = x.AlternatePhone, CertificateNo = x.CertificateNo, CheckedInStatus = x.CheckedInStatus, ClientID = x.ClientID, Codes = x.Codes, Comments = x.Comments, DeliveryOption = x.DeliveryOption, Email = x.Email, ExtraClassTimes = x.ExtraClassTimes, FirstName = x.FirstName, IsBillingSameAsMailing = x.IsBillingSameAsMailing, LastName = x.LastName, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, PrimaryPhone = x.PrimaryPhone, PromoCode = x.PromoCode, Remediation = x.Remediation, SelectedOptions = x.SelectedOptions, StatusID = x.StatusID, StudentID = x.StudentID, TestScore = x.TestScore, RegisterionDate = x.RegistrationDate }).ToList();
            byte[] pdfBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.A4, 40, 40, 50, 50))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        var titleFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                        var boldTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                        var bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                        var smallfont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
                        var footerFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);

                        iTextSharp.text.Rectangle pageSize = w.PageSize;

                        doc.Open();

                        PdfPTable headertable = new PdfPTable(1);
                        headertable.WidthPercentage = 100;
                        headertable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        headertable.SpacingAfter = 30;

                        PdfPCell courseCell = new PdfPCell(new Phrase(students[0].CourseName, titleFont));
                        courseCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        headertable.AddCell(courseCell);

                        PdfPCell classtime = new PdfPCell(new Phrase(students[0].ScheduleDate.ToString("dddd M/d/yyyy") + " from " + students[0].StartTime + " to " + students[0].EndTime, bodyFont));
                        classtime.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        headertable.AddCell(classtime);

                        PdfPCell instructorCell = new PdfPCell(new Phrase("Instructor: " , bodyFont));
                        instructorCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        headertable.AddCell(instructorCell);

                        doc.Add(headertable);

                        #region Students Detail
                        //Create body table
                        PdfPTable itemTable = new PdfPTable(6);
                        itemTable.HorizontalAlignment = 0;
                        itemTable.WidthPercentage = 100;
                        itemTable.SetWidths(new float[] { 5, 20, 15, 15, 20, 20 });  
                                                                                    
                        itemTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
                        PdfPCell cell4 = new PdfPCell(new Phrase("#", boldTableFont));
                        cell4.HorizontalAlignment = 1;
                        itemTable.AddCell(cell4);

                        PdfPCell cell1 = new PdfPCell(new Phrase("Name", boldTableFont));
                        cell1.HorizontalAlignment = 1;
                        itemTable.AddCell(cell1);

                        PdfPCell cell6 = new PdfPCell(new Phrase("Codes", boldTableFont));
                        cell6.HorizontalAlignment = 1;
                        itemTable.AddCell(cell6);

                        PdfPCell cell2 = new PdfPCell(new Phrase("Phone", boldTableFont));
                        cell2.HorizontalAlignment = 1;
                        itemTable.AddCell(cell2);
                        PdfPCell cell5 = new PdfPCell(new Phrase("Options", boldTableFont));
                        cell5.HorizontalAlignment = 1;
                        itemTable.AddCell(cell5);

                        PdfPCell cell3 = new PdfPCell(new Phrase("Notes", boldTableFont));
                        cell3.HorizontalAlignment = 1;
                        itemTable.AddCell(cell3);

                        int srno = 0;

                        foreach (var student in students)
                        {
                            srno = srno + 1; ;
                            var addonns = string.IsNullOrWhiteSpace(student.SelectedOptions) ? "" : student.SelectedOptions;
                            addonns = string.Join(",", db.RegistrationAdOn.Where(e => addonns.Contains(e.AdOnID.ToString())).ToList().Select(e => e.Name).ToList());

                            PdfPCell qty = new PdfPCell(new Phrase(srno.ToString(), bodyFont));
                            qty.HorizontalAlignment = 1;
                            qty.PaddingLeft = 10f;
                            qty.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(qty);


                            PdfPCell sku = new PdfPCell(new Phrase(student.LastName + "," + student.FirstName, bodyFont));
                            sku.HorizontalAlignment = 0;
                            sku.PaddingLeft = 10f;
                            sku.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(sku);

                            PdfPCell upc = new PdfPCell(new Phrase(student.Codes, bodyFont));
                            upc.HorizontalAlignment = 0;
                            upc.PaddingLeft = 10f;
                            upc.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(upc);

                            PdfPCell description = new PdfPCell(new Phrase(student.PrimaryPhone, bodyFont));
                            description.HorizontalAlignment = 0;
                            description.PaddingLeft = 10f;
                            description.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(description);


                            PdfPCell each = new PdfPCell(new Phrase(addonns, bodyFont));
                            each.PaddingRight = 4f;
                            each.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(each);

                            PdfPCell price = new PdfPCell(new Phrase(student.Comments, bodyFont));
                            price.PaddingRight = 4f;
                            price.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            itemTable.AddCell(price);

                        }

                        doc.Add(itemTable);
                        #endregion
                        doc.Close();
                    }
                }

                pdfBytes = ms.ToArray();
            }

            return File(pdfBytes, "application/pdf");
        }



        public ActionResult CertificatePrinting(int id)
        {
            var students = (from schedule in db.ScheduleClass
                                  join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                  join student in db.Student on schedule.ClassID equals student.ClassID
                                  where schedule.ClassID == id && student.IsUnSchedule == false && schedule.CompanyID == SessionWrapper.Current.CompanyID
                                  select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, AlternatePhone = student.AlternatePhone, CertificateNo = student.CertificateNo, CheckedInStatus = student.CheckedInStatus, ClientID = student.ClientID, Codes = student.Codes, Comments = student.Comments, DeliveryOption = student.DeliveryOption, Email = student.Email, ExtraClassTimes = schedule.ExtraClassTimes, FirstName = student.FirstName, IsBillingSameAsMailing = student.IsBillingSameAsMailing, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, PromoCode = student.PromoCode, Remediation = student.Remediation, SelectedOptions = student.SelectedOptions, StatusID = student.StatusID, StudentID = student.StudentID, TestScore = student.TestScore, RegistrationDate = student.RegisterionDate, ClassHours = schedule.ClassDuration, CEU = course.CEUCredits, IssueDate = DateTime.Now }).ToArray();

            CertificatePrintViewModel certificates = new CertificatePrintViewModel();
            if (students != null && students.Length > 0)
            {
                certificates.ClassID = students[0].ClassID;
                certificates.CourseName = students[0].CourseName;
                certificates.ScheduleDate = students[0].ScheduleDate;
                certificates.StartTime = students[0].StartTime;
                certificates.EndTime = students[0].EndTime;
                certificates.CEUCredits =  students[0].CEU == null ? "" : students[0].CEU.ToString();
                certificates.IssueDate = students[0].IssueDate.ToString("M/d/yyyy");
                certificates.ClassHours = students[0].ClassHours;


                certificates.StudentID = new int[students.Length];
                certificates.FirstName = new string[students.Length];
                certificates.LastName = new string[students.Length];
                for (int i = 0; i < students.Length; i++)
                {
                    certificates.StudentID[i] = students[i].StudentID;
                    certificates.FirstName[i] = students[i].FirstName;
                    certificates.LastName[i] = students[i].LastName;
                }
            }

            return View(certificates);
        }


        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult StudentCerificate(CertificatePrintViewModel model)
        {

            MemoryStream stream = new MemoryStream();
            DocX certificates = DocX.Create(stream);
            string templatepath = "/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/" + model.Template;
            string serverpath = Server.MapPath(templatepath);
            if (System.IO.File.Exists(serverpath))
            {
                DocX template = DocX.Load(serverpath);
                certificates.PageHeight = template.PageHeight;
                certificates.PageWidth = template.PageWidth;
                certificates.PageLayout.Orientation = template.PageLayout.Orientation;
                DocX clone = template.Copy();
                int totalSelectedStudents = Array.FindAll(model.SelectedStudent, delegate (bool b) { return b == true; }).Length;
                int page = 0;
                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        page = page + 1;
                        template.ReplaceText("[STUDENT]", (string.IsNullOrWhiteSpace(model.LastName[i]) ? "" : model.LastName[i]) + " " + (string.IsNullOrWhiteSpace(model.FirstName[i]) ? "" : model.FirstName[i]));
                        template.ReplaceText("[COURSE]", string.IsNullOrWhiteSpace(model.CourseName) ? "" : model.CourseName);
                        template.ReplaceText("[CREDITS]", string.IsNullOrWhiteSpace(model.CEUCredits) ? "" : model.CEUCredits);
                        template.ReplaceText("[DATE]", string.IsNullOrWhiteSpace(model.IssueDate) ? "" : model.IssueDate);
                        template.ReplaceText("[INSTRUCTOR]", string.IsNullOrWhiteSpace(model.Instructor) ? "" : model.Instructor);
                        template.ReplaceText("[LICENSE]", "");

                        certificates.InsertDocument(template, true);
                        if (page != totalSelectedStudents)
                        {
                            certificates.InsertParagraph().InsertPageBreakAfterSelf();
                        }
                        template = clone;
                        clone = template.Copy();
                    }
                }
                certificates.Save();
                return File(stream.ToArray(), "application/octet-stream", "StudentCertificates.docx");
            }
            else
            {
                return Content("Certificate Template not found");
            }
        }


        public ActionResult CardPrinting(int id)
        {
            var students = (from schedule in db.ScheduleClass
                            join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                            join student in db.Student on schedule.ClassID equals student.ClassID
                            where schedule.ClassID == id && student.IsUnSchedule == false && schedule.CompanyID == SessionWrapper.Current.CompanyID
                            select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, AlternatePhone = student.AlternatePhone, Email = student.Email, FirstName = student.FirstName, LastName = student.LastName, MailingAddress1 = student.MailingAddress1, MailingAddress2 = student.MailingAddress2, MailingCity = student.MailingCity, MailingState = student.MailingState, MailingZip = student.MailingZip, PrimaryPhone = student.PrimaryPhone, StudentID = student.StudentID, RegistrationDate = student.RegisterionDate, IssueDate = DateTime.Now }).ToArray();

            CardPrintViewModel cards = new CardPrintViewModel();
            if (students != null && students.Length > 0)
            {
                cards.ClassID = students[0].ClassID;
                cards.CourseName = students[0].CourseName;
                cards.ScheduleDate = students[0].ScheduleDate;
                cards.StartTime = students[0].StartTime;
                cards.EndTime = students[0].EndTime;
                cards.IssueDate = students[0].IssueDate.ToString("M/d/yyyy");


                cards.StudentID = new int[students.Length];
                cards.FirstName = new string[students.Length];
                cards.LastName = new string[students.Length];
                cards.Address1 = new string[students.Length];
                cards.Address2 = new string[students.Length];
                cards.City = new string[students.Length];
                cards.State = new string[students.Length];
                cards.Zip = new string[students.Length];
                for (int i = 0; i < students.Length; i++)
                {
                    cards.StudentID[i] = students[i].StudentID;
                    cards.FirstName[i] = students[i].FirstName;
                    cards.LastName[i] = students[i].LastName;
                    cards.Address1[i] = students[i].MailingAddress1;
                    cards.Address2[i] = students[i].MailingAddress2;
                    cards.City[i] = students[i].MailingCity;
                    cards.State[i] = students[i].MailingState;
                    cards.Zip[i] = students[i].MailingZip;
                }
            }

            //var cardsetting = db.CardSetting.Where(x => x.CompanyID == Common.SessionWrapper.Current.CompanyID);

            return View(cards);
        }


        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult StudentCards(CardPrintViewModel model)
        {
            MemoryStream stream = new MemoryStream();
            DocX cards = DocX.Create(stream);

            EnumModel.CardType CardType = (EnumModel.CardType)model.CardType;

            if (CardType == EnumModel.CardType.AHA2005 || CardType == EnumModel.CardType.AHAInstructor3 || CardType == EnumModel.CardType.AHAProvider3)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(model.TrainingCenter);
                sb.AppendLine(model.TCInfo1);
                sb.AppendLine(model.TCInfo2);
                sb.AppendLine();
                sb.AppendLine(model.CourseLocation1);
                sb.AppendLine(model.CourseLocation2);
                sb.AppendLine();
                sb.AppendLine(model.Instructor);

                string s = sb.ToString();
                int record = 1;
                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = Environment.NewLine + Environment.NewLine + model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + Environment.NewLine + model.IssueDate + "            " + model.Expires;
                        Table t = cards.AddTable(1, 2);
                        Border b = new Border();
                        b.Space = 0;
                        b.Color = Color.Transparent;
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);
                        t.SetBorder(TableBorderType.InsideH, b);
                        t.SetBorder(TableBorderType.InsideV, b);

                        t.SetWidths(new float[] { 500f, 400f });
                        t.Alignment = Alignment.center;

                        t.Design = TableDesign.TableNormal;
                        t.Rows[0].Cells[0].Paragraphs.First().Append(stInfo).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[0].Cells[1].Paragraphs.First().Append(Regex.Replace(s, @"\r", "")).Font(new FontFamily("Arial")).FontSize(10);

                        cards.InsertTable(t);


                        if (record % model.CardOnPage == 0)
                        {
                            cards.InsertParagraph().InsertPageBreakAfterSelf();
                        }
                        else
                        {
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                        }
                        record++;
                    }
                }
            }
            else if (CardType == EnumModel.CardType.AHAInstructor2 || CardType == EnumModel.CardType.AHAProvider2)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(model.TrainingCenter);
                sb.AppendLine(model.TCInfo1);
                sb.AppendLine(model.TCInfo2);
                sb.AppendLine();
                sb.AppendLine(model.CourseLocation1);
                sb.AppendLine(model.CourseLocation2);
                sb.AppendLine();
                sb.AppendLine(model.Instructor);

                string s = sb.ToString();
                int record = 1;
                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = Environment.NewLine + Environment.NewLine + model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + Environment.NewLine + model.IssueDate + "            " + model.Expires + Environment.NewLine + Environment.NewLine + model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + model.Address1[i] + Environment.NewLine + (string.IsNullOrWhiteSpace(model.Address2[i])? "" : model.Address2[i] + Environment.NewLine) + model.City[i] + ", " + model.State[i] + " " + model.Zip[i];
                        Table t = cards.AddTable(1, 2);
                        Border b = new Border();
                        b.Space = 0;
                        b.Color = Color.Transparent;
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);
                        t.SetBorder(TableBorderType.InsideH, b);
                        t.SetBorder(TableBorderType.InsideV, b);

                        t.SetWidths(new float[] { 500f, 400f });
                        t.Alignment = Alignment.center;

                        t.Design = TableDesign.TableNormal;
                        t.Rows[0].Cells[0].Paragraphs.First().Append(stInfo).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[0].Cells[1].Paragraphs.First().Append(Regex.Replace(s, @"\r", "")).Font(new FontFamily("Arial")).FontSize(10);

                        cards.InsertTable(t);


                        if (record % model.CardOnPage == 0)
                        {
                            cards.InsertParagraph().InsertPageBreakAfterSelf();
                        }
                        else
                        {
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                            cards.InsertParagraph(Environment.NewLine);
                        }
                        record++;
                    }
                }
            }
            else if (CardType == EnumModel.CardType.ASHI2010 || CardType == EnumModel.CardType.ASHI2015)
            {
                int record = 1;
                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = Environment.NewLine + Environment.NewLine + model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + Environment.NewLine + model.IssueDate + "            " + model.Expires + Environment.NewLine + Environment.NewLine + model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + model.Address1[i] + Environment.NewLine + (string.IsNullOrWhiteSpace(model.Address2[i]) ? "" : model.Address2[i] + Environment.NewLine) + model.City[i] + ", " + model.State[i] + " " + model.Zip[i];
                        Table t = cards.AddTable(4, 4);
                        Border b = new Border();
                        b.Space = 0;
                        b.Color = Color.Transparent;
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);
                        t.SetBorder(TableBorderType.InsideH, b);
                        t.SetBorder(TableBorderType.InsideV, b);

                        t.SetWidths(new float[] { 350f, 50f, 250f, 250f });
                        t.Alignment = Alignment.center;

                        t.Design = TableDesign.TableNormal;
                        t.Rows[0].MergeCells(2, 3);
                        t.Rows[1].MergeCells(2, 3);

                        t.Rows[0].Cells[2].Paragraphs.First().Append(model.Instructor).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[1].Cells[2].Paragraphs.First().Append("ASHI1234").Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[2].Cells[0].Paragraphs.First().Append(model.LastName[i] + " " + model.FirstName[i]).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[2].Cells[2].Paragraphs.First().Append(model.IssueDate).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[2].Cells[2].Paragraphs.First().Append(model.Expires).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[3].Cells[2].Paragraphs.First().Append(model.TCInfo2).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[3].Cells[3].Paragraphs.First().Append(model.TrainingCenter).Font(new FontFamily("Arial")).FontSize(10);
                        cards.InsertTable(t);

                        cards.InsertParagraph(Environment.NewLine);
                        cards.InsertParagraph(Environment.NewLine);
                        cards.InsertParagraph(Environment.NewLine);

                        record++;
                    }
                }
            }
            else if (CardType == EnumModel.CardType.AveryLabels)
            {
                int record = 1;
                int totalSelectedStudents = Array.FindAll(model.SelectedStudent, delegate (bool c) { return c == true; }).Length;
                double rows = Math.Ceiling(Convert.ToDouble(totalSelectedStudents) / 3);
                Table t = cards.AddTable( Convert.ToInt32(rows), 3);

                Border b = new Border();
                b.Space = 0;
                b.Color = Color.Transparent;
                t.SetBorder(TableBorderType.Bottom, b);
                t.SetBorder(TableBorderType.Top, b);
                t.SetBorder(TableBorderType.Left, b);
                t.SetBorder(TableBorderType.Right, b);
                t.SetBorder(TableBorderType.InsideH, b);
                t.SetBorder(TableBorderType.InsideV, b);

                t.SetWidths(new float[] { 300f, 300f, 300f });
                t.Alignment = Alignment.center;
                t.SetTableCellMargin(TableCellMarginType.top, 20);
                t.SetTableCellMargin(TableCellMarginType.bottom, 20);
                t.Design = TableDesign.TableNormal;

                int movingcell = 0;
                int movingrow = 0;

                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + model.Address1[i] + Environment.NewLine + (string.IsNullOrWhiteSpace(model.Address2[i]) ? "" : model.Address2[i] + Environment.NewLine) + model.City[i] + ", " + model.State[i] + " " + model.Zip[i] + Environment.NewLine + Environment.NewLine;
                        t.Rows[movingrow].Cells[movingcell].Paragraphs.First().Append(Regex.Replace(stInfo, @"\r", "")).Font(new FontFamily("Arial")).FontSize(10);
                        if (movingcell == 2)
                        {
                            movingcell = 0;
                        }
                        else
                        {
                            movingcell = movingcell + 1;
                        }

                        if (record % 3 == 0)
                        {
                            movingrow = movingrow + 1;
                        }
                        record++;
                    }
                }
                cards.InsertTable(t);
            }
            else if (CardType == EnumModel.CardType.AveryTags)
            {
                int record = 1;
                int totalSelectedStudents = Array.FindAll(model.SelectedStudent, delegate (bool c) { return c == true; }).Length;
                double rows = Math.Ceiling(Convert.ToDouble(totalSelectedStudents) / 2);
                Table t = cards.AddTable(Convert.ToInt32(rows), 2);

                Border b = new Border();
                b.Space = 0;
                b.Color = Color.Transparent;
                t.SetBorder(TableBorderType.Bottom, b);
                t.SetBorder(TableBorderType.Top, b);
                t.SetBorder(TableBorderType.Left, b);
                t.SetBorder(TableBorderType.Right, b);
                t.SetBorder(TableBorderType.InsideH, b);
                t.SetBorder(TableBorderType.InsideV, b);

                t.SetWidths(new float[] { 450f, 450f});
                t.Alignment = Alignment.center;
                t.SetTableCellMargin(TableCellMarginType.top, 20);
                t.SetTableCellMargin(TableCellMarginType.bottom, 20);
                t.Design = TableDesign.TableNormal;

                int movingcell = 0;
                int movingrow = 0;

                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                        t.Rows[movingrow].Cells[movingcell].Paragraphs.First().Append(Regex.Replace(stInfo, @"\r", "")).Font(new FontFamily("Calibri")).FontSize(28).Bold();
                        if (movingcell == 1)
                        {
                            movingcell = 0;
                        }
                        else
                        {
                            movingcell = movingcell + 1;
                        }

                        if (record % 2 == 0)
                        {
                            movingrow = movingrow + 1;
                        }
                        record++;
                    }
                }
                cards.InsertTable(t);
            }
            else if (CardType == EnumModel.CardType.ESCI)
            {
                int record = 1;
                //cards.MarginTop = 1.28f;
                //cards.MarginBottom = 0.63f;
                //cards.MarginLeft = 30f;
                //cards.MarginRight = 20f;
                int totalSelectedStudents = Array.FindAll(model.SelectedStudent, delegate (bool c) { return c == true; }).Length;
                double rows = Math.Ceiling(Convert.ToDouble(totalSelectedStudents) / 2);
                Table t = cards.AddTable(Convert.ToInt32(rows), 2);

                Border b = new Border();
                b.Space = 0;
                b.Color = Color.Transparent;
                t.SetBorder(TableBorderType.Bottom, b);
                t.SetBorder(TableBorderType.Top, b);
                t.SetBorder(TableBorderType.Left, b);
                t.SetBorder(TableBorderType.Right, b);
                t.SetBorder(TableBorderType.InsideH, b);
                t.SetBorder(TableBorderType.InsideV, b);

                t.SetWidths(new float[] { 500f, 500f });
                t.Alignment = Alignment.center;
                t.SetTableCellMargin(TableCellMarginType.top, 20);
                t.SetTableCellMargin(TableCellMarginType.bottom, 20);
                t.Design = TableDesign.TableNormal;


                int movingcell = 0;
                int movingrow = 0;

                for (int i = 0; i < model.SelectedStudent.Length; i++)
                {
                    if (model.SelectedStudent[i] == true)
                    {
                        string stInfo = model.LastName[i] + " " + model.FirstName[i] + Environment.NewLine + Environment.NewLine + "ECSI Course" + Environment.NewLine + Environment.NewLine;
                        string extra = model.IssueDate + "\t" + model.Expires + Environment.NewLine + model.Instructor + "\t" + model.TrainingCenter + "\t" + model.TCInfo2 + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                        t.Rows[movingrow].Cells[movingcell].Paragraphs.First().Append(Regex.Replace(stInfo, @"\r", "")).Font(new FontFamily("Arial")).FontSize(10);
                        t.Rows[movingrow].Cells[movingcell].Paragraphs.First().Append(Regex.Replace(extra, @"\r", "")).Font(new FontFamily("Arial")).FontSize(6);
                        if (movingcell == 1)
                        {
                            movingcell = 0;
                        }
                        else
                        {
                            movingcell = movingcell + 1;
                        }

                        if (record % 2 == 0)
                        {
                            movingrow = movingrow + 1;
                        }
                        record++;
                    }
                }
                cards.InsertTable(t);
            }
            cards.Save();
            return File(stream.ToArray(), "application/octet-stream", "CardPrints.docx");
        }

        public ActionResult RepeatClass(int id)
        {
            ScheduleClass scheduleclass = (from schedule in db.ScheduleClass
                               join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                               join location in db.Location on schedule.LocationID equals location.LocationID
                               where schedule.ClassID == id
                               select new { ClassID = schedule.ClassID, CourseName = course.CourseName, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, Location = location.Name == null ? "" : location.Name})
                  .AsEnumerable().Select(x => new ScheduleClass { ClassID = x.ClassID, CourseName = x.CourseName, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, Location = x.Location}).FirstOrDefault();
            return View(scheduleclass);
        }

        [HttpPost]
        public ActionResult RepeatClass(ScheduleClass model)
        {
            try
            {

                int totalRepeats = Array.FindAll(model.RepeatDates, delegate (string c) { return !string.IsNullOrWhiteSpace(c) ; }).Length;
                if (totalRepeats > 0)
                {
                    ScheduleClass SourceClass = db.ScheduleClass.Find(model.ClassID);
                    List<ScheduleClass> RepeatClasses = new List<Models.ScheduleClass>();
                    int ClassNo = 0;
                    if (SessionWrapper.Current.ClassNumberSetting == 1) // Auto Assigned
                    {
                        ClassNo = db.ScheduleClass.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID).Max(x => x.ClassNo).GetValueOrDefault() + 1;
                    }

                    for (int i = 0; i < model.RepeatDates.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(model.RepeatDates[i]))
                        {

                            List<string> lstRepeatDates = model.RepeatDates[i].Split(new string[] { "," }, StringSplitOptions.None).ToList();
                            foreach (string repeatDate in lstRepeatDates)
                            {
                                ScheduleClass repeat = new ScheduleClass();
                                repeat.ScheduleDate = Convert.ToDateTime(repeatDate);
                                repeat.StartTime = SourceClass.StartTime;
                                repeat.EndTime = SourceClass.EndTime;
                                repeat.ClassDuration = SourceClass.ClassDuration;
                                repeat.CompanyID = SourceClass.CompanyID;
                                repeat.CourseID = SourceClass.CourseID;
                                repeat.IncludeOnline = SourceClass.IncludeOnline;
                                repeat.InstructorID = SourceClass.InstructorID;
                                repeat.LocationID = SourceClass.LocationID;
                                repeat.MaxStudents = SourceClass.MaxStudents;
                                repeat.Price = SourceClass.Price;
                                repeat.PublicNotes = SourceClass.PublicNotes;
                                repeat.StudentManikinRatio = SourceClass.StudentManikinRatio;
                                repeat.ClientID = SourceClass.ClientID;
                                repeat.InstructorID = SourceClass.InstructorID;

                                if (SessionWrapper.Current.ClassNumberSetting == 1) // Auto Assigned
                                {
                                    repeat.ClassNo = ClassNo;
                                }
                                ClassNo = ClassNo + 1;

                                RepeatClasses.Add(repeat);
                                
                            }


                        }

                    }

                    int rowsAffected = 0;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.ScheduleClass.AddRange(RepeatClasses);
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Successfully Scheduled the repeat classes");
                        return RedirectToAction("UpcomingClasses", "Admin");
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }

                }
                else
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger("No Date Selected to Repeat the class");
                }

            }
            catch (Exception ex)
            {
                FlashMessage.Retrieve(ControllerContext.HttpContext);
                FlashMessage.Danger(ex.Message);
            }


            return View(model);
        }


        public ActionResult KeycodeSales()
        {
            DateTime Last12Months = DateTime.Now.Date.AddMonths(-12);

            IEnumerable<KeycodeSaleViewModel> KeycodeSales = (from student in db.Student 
                                                     join courseAddon in db.CourseAdon on student.StudentID equals courseAddon.StudentID
                                                     join schedule in db.ScheduleClass on student.ClassID equals schedule.ClassID
                                                     join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                                     where schedule.CompanyID == SessionWrapper.Current.CompanyID && courseAddon.Type == "KeyCode" && courseAddon.RegisterDate >= Last12Months && courseAddon.RegisterDate <= DateTime.Today
                                                     select new KeycodeSaleViewModel { ClassID = schedule.ClassID, ClassDate = schedule.ScheduleDate, ClassTime = schedule.StartTime, Course = course.CourseName, Email = student.Email, FirstName = student.FirstName, Keycode = courseAddon.Keycode, LastName = student.LastName, Phone = student.PrimaryPhone, Purchased = courseAddon.RegisterDate, StudentID = student.StudentID, IsUnscheduled = student.IsUnSchedule });

            return View(KeycodeSales);
        }

        [HttpPost]
        public ActionResult KeycodeSales(string ksFilter)
        {

            var keycodesales = (from student in db.Student
                                join courseAddon in db.CourseAdon on student.StudentID equals courseAddon.StudentID
                                join schedule in db.ScheduleClass on student.ClassID equals schedule.ClassID
                                join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                where schedule.CompanyID == SessionWrapper.Current.CompanyID && courseAddon.Type == "KeyCode"
                                select new KeycodeSaleViewModel { ClassID = schedule.ClassID, ClassDate = schedule.ScheduleDate, ClassTime = schedule.StartTime, Course = course.CourseName, Email = student.Email, FirstName = student.FirstName, Keycode = courseAddon.Keycode, LastName = student.LastName, Phone = student.PrimaryPhone, Purchased = courseAddon.RegisterDate, StudentID = student.StudentID, IsUnscheduled = student.IsUnSchedule });

            if (ksFilter == "Last 12 Months")
            {
                DateTime Last12Months = DateTime.Now.Date.AddMonths(-12);
                keycodesales = keycodesales.Where(x => x.Purchased >= Last12Months && x.Purchased <= DateTime.Today);
            }
            else if(ksFilter == "Last 24 Months")
            {
                DateTime Last24Months = DateTime.Now.Date.AddMonths(-24);
                keycodesales = keycodesales.Where(x => x.Purchased >= Last24Months && x.Purchased <= DateTime.Today);
            }
            ViewBag.SelectedFilter = ksFilter;
            return View(keycodesales);
        }


        public ActionResult Instructors()
        {
            IEnumerable<User> Instructors =  db.User.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && (x.IsInstructor == true || x.IsInstructorAssistant == true));
            return View(Instructors);
        }

        public ActionResult EditInstructor(int id)
        {
            User instructor = db.User.Find(id);
            return View(instructor);
        }

        [HttpPost]
        public ActionResult InstructorUpdateBasic(User model)
        {
            db.User.Attach(model);
            var entry = db.Entry(model);
            entry.Property(e => e.MonitorDate).IsModified = true;
            entry.Property(e => e.DuesPaidThru).IsModified = true;
            entry.Property(e => e.Notes).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            FlashMessage.Confirmation("Record Updated Successfully");
            return RedirectToAction("Instructors", "Admin");

        }


        [HttpPost]
        public ActionResult UploadInstructorDocuments(int id)
        {
            foreach (string file in Request.Files)
            {
                try
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf != null && !string.IsNullOrWhiteSpace(hpf.FileName))
                    {
                        string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/instructordocs/" + id + "/";
                        SaveFile(hpf, dirpath);
                        return Json(1, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception ex)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstructorDocs(int id)
        {
            string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/instructordocs/" + id + "/";
            List<string> lstdocs = Common.Utilities.GetCertificates(dirpath);
            return PartialView(lstdocs);
        }

        public ActionResult DeleteInstructorDocs(int id, string FileName)
        {
            try
            {
                string pathstr = "/uploads/" + SessionWrapper.Current.SubDomain + "/instructordocs/" + id + "/" + FileName;
                string filepath = Server.MapPath(pathstr);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    return RedirectToAction("GetInstructorDocs", "Admin", new { id = id });
                }
                else
                {
                    return Content("File Not Exist");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }



        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult InstructorCourses(DateTime fromDate, DateTime toDate, int InstructorID)
        {
            ViewBag.InsID = InstructorID;
            IEnumerable<InstructorClasses> InstructorClasses = (from schedule in db.ScheduleClass
                         join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                         join location in db.Location on schedule.LocationID equals location.LocationID
                         where schedule.ScheduleDate >= fromDate && schedule.ScheduleDate <= toDate && schedule.CompanyID == SessionWrapper.Current.CompanyID && (schedule.InstructorID == InstructorID || schedule.AssistantID == InstructorID)
                         select new InstructorClasses { CourseID = course.CourseTypeID,  ClassVenue= location.Name, AssistantID = schedule.AssistantID, ClassDate = schedule.ScheduleDate, InstructorID = schedule.InstructorID, ClassID = schedule.ClassID, Course = course.CourseName, RegisteredStudents = db.Student.Where(x => x.ClassID == schedule.ClassID && x.IsUnSchedule == false).Count() });
            return PartialView(InstructorClasses);
        }

        public ActionResult AddInstructorCertification(int id)
        {
            User oUser = db.User.Find(id);
            string name = "";
            if (oUser!= null)
            {
                name = oUser.FirstName + " " + oUser.LastName;
            }
            ViewBag.id = id;
            ViewBag.name = name;
            return PartialView();
        }

        public ActionResult InstructorCertifications(int InstructorID = 0)
        {
            IEnumerable<EnrolTraining.Models.InstructorCertification> certifications = db.InstructorCertification.Where(x => x.InstructorID == InstructorID);
            return PartialView(certifications);
        }
        public ActionResult EditInstructorCertification(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorCertification oCertification = db.InstructorCertification.Find(id);
            if (oCertification == null)
            {
                return HttpNotFound();
            }
            User oUser = db.User.Find(oCertification.InstructorID);
            string name = "";
            if (oUser != null)
            {
                name = oUser.FirstName + " " + oUser.LastName;
            }
            ViewBag.id = oCertification.InstructorID;
            ViewBag.name = name;

            return PartialView("AddInstructorCertification", oCertification);
        }

        [HttpPost]
        public ActionResult AddInstructorCertification(InstructorCertification model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    if (model.CertificationID > 0)
                    {
                        db.InstructorCertification.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //FlashMessage.Confirmation("Record Successfully Updated");
                            //return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID});
                            return Json(new { success = true });
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.InstructorCertification.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //FlashMessage.Confirmation("Record Successfully Added");
                            //return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
                            return Json(new { success = true });
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return PartialView(model);
        }

        public ActionResult DeleteInstructorCertification(int id)
        {
            var model = db.InstructorCertification.Find(id); 

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.InstructorCertification.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID});
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
        }

        public ActionResult AddInstructorActivity(int id)
        {
            User oUser = db.User.Find(id);
            string name = "";
            if (oUser != null)
            {
                name = oUser.FirstName + " " + oUser.LastName;
            }
            ViewBag.id = id;
            ViewBag.name = name;
            return PartialView();
        }

        public ActionResult InstructorActivities(int InstructorID = 0)
        {
            IEnumerable<EnrolTraining.Models.InstructorActivityLog> activities = db.InstructorActivityLog.Where(x => x.InstructorID == InstructorID);
            return PartialView(activities);
        }

        public ActionResult EditInstructorActivity(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorActivityLog oActivity = db.InstructorActivityLog.Find(id);
            if (oActivity == null)
            {
                return HttpNotFound();
            }
            User oUser = db.User.Find(oActivity.InstructorID);
            string name = "";
            if (oUser != null)
            {
                name = oUser.FirstName + " " + oUser.LastName;
            }
            ViewBag.id = oActivity.InstructorID;
            ViewBag.name = name;

            return PartialView("AddInstructorActivity", oActivity);
        }

        [HttpPost]
        public ActionResult AddInstructorActivity(InstructorActivityLog model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    if (model.ActivityID > 0)
                    {
                        db.InstructorActivityLog.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //FlashMessage.Confirmation("Record Successfully Updated");
                            //return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
                            return Json(new { success = true });
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.InstructorActivityLog.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            //FlashMessage.Confirmation("Record Successfully Added");
                            //return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
                            return Json(new { success = true });
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return PartialView(model);
        }


        public ActionResult DeleteInstructorActivity(int id)
        {
            var model = db.InstructorActivityLog.Find(id);

            if (model != null)
            {
                try
                {
                    int rowsAffected = 0;
                    db.InstructorActivityLog.Attach(model);
                    var entity = db.Entry(model);
                    entity.State = System.Data.Entity.EntityState.Deleted;
                    rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        FlashMessage.Confirmation("Record Successfully Deleted");
                        return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
                    }
                    else
                    {
                        FlashMessage.Retrieve(ControllerContext.HttpContext);
                        FlashMessage.Danger("Something Goes Wrong");
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }
            }
            FlashMessage.Confirmation("Something Goes Wrong");
            return RedirectToAction("EditInstructor", "Admin", new { id = model.InstructorID });
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ExpiringCertifications(int days = 90)
        {
            ViewBag.days = days;
            IEnumerable<User> Instructors = db.User.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && (x.IsInstructor == true || x.IsInstructorAssistant == true));
            return View(Instructors);
        }


        public ActionResult AddClient()
        {
            return View();
        }

        public ActionResult EditClient(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client oClient = db.Client.Find(id);
            if (oClient == null)
            {
                return HttpNotFound();
            }
            return View("AddClient", oClient);
        }

        [HttpPost]
        public ActionResult AddClient(Client model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsAffected = 0;
                    model.CompanyID = SessionWrapper.Current.CompanyID;

                    if (model.ClientID > 0)
                    {
                        db.Client.Attach(model);
                        var entry = db.Entry(model);
                        entry.State = System.Data.Entity.EntityState.Modified;
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Updated");
                            return RedirectToAction("Clients", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }

                    }
                    else
                    {
                        db.Client.Add(model);
                        rowsAffected = db.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            FlashMessage.Confirmation("Record Successfully Added");
                            return RedirectToAction("AddClient", "Admin");
                        }
                        else
                        {
                            FlashMessage.Retrieve(ControllerContext.HttpContext);
                            FlashMessage.Danger("Something Goes Wrong");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult Clients()
        {
            return View(db.Client.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID));
        }

        [HttpPost]
        public ActionResult UploadClientDocuments(int ClientID)
        {
            foreach (string file in Request.Files)
            {
                try
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf != null && !string.IsNullOrWhiteSpace(hpf.FileName))
                    {
                        string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/clientdocs/" + ClientID + "/";
                        SaveFile(hpf, dirpath);
                        return Json(1, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception ex)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClientDocs(int ClientID)
        {
            string dirpath = "/uploads/" + SessionWrapper.Current.SubDomain + "/clientdocs/" + ClientID + "/";
            List<string> lst = Common.Utilities.GetCertificates(dirpath);
            return PartialView(lst);
        }

        public ActionResult DeleteClientDocs(int ClientID, string FileName)
        {
            try
            {
                string pathstr = "/uploads/" + SessionWrapper.Current.SubDomain + "/clientdocs/" + ClientID + "/" + FileName;
                string filepath = Server.MapPath(pathstr);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    return RedirectToAction("GetClientDocs", "Admin", new { ClientID = ClientID });
                }
                else
                {
                    return Content("File Not Exist");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }


        public ActionResult ClientActivity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClientActivity(ClientActivityFilter model)
        {
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult SearchClientActivities(ClientActivityFilter model)
        {

            IEnumerable<ClientActivityViewModel> activities = (from schedule in db.ScheduleClass
                                                               join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                                               join student in db.Student on schedule.ClassID equals student.ClassID
                                                               where schedule.CompanyID == SessionWrapper.Current.CompanyID && schedule.ClientID == model.ClinetID
                                                               select new ClientActivityViewModel { StudentID = student.StudentID, Email = student.Email, FirstName = student.FirstName, LastName = student.LastName, CourseName = course.CourseName, Price = schedule.Price, ScheduleDate = schedule.ScheduleDate, ScheduleTime = schedule.StartTime, Status = student.StatusID});

            int month = 0, year = 0;
            if (model.DateCriteria == "This Month")
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
                activities = activities.Where(x => x.ScheduleDate.Month == month && x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Last Month")
            {
                DateTime dt = DateTime.Now.AddMonths(-1);
                month = dt.Month;
                year = dt.Year;
                activities = activities.Where(x => x.ScheduleDate.Month == month && x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "This Year")
            {
                year = DateTime.Today.Year;
                activities = activities.Where(x => x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Last Year")
            {
                DateTime dt = DateTime.Now.AddYears(-1);
                year = dt.Year;
                activities = activities.Where(x => x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Custom Range")
            {
                activities = activities.Where(x => x.ScheduleDate >= model.DateForm.Date && x.ScheduleDate <= model.DateTo.Date);
            }

            return View(activities);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User oUser = db.User.Where(u => u.UserID == SessionWrapper.Current.UserID && u.Password == model.CurrentPassword).FirstOrDefault();
                    if (oUser != null) 
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        oUser.Password = model.NewPassword;
                        db.User.Attach(oUser);
                        var entry = db.Entry(oUser);
                        entry.Property(e => e.Password).IsModified = true;
                        db.SaveChanges();
                        FlashMessage.Confirmation("Password Successfully Chnaged. Please Login with new password");
                        return RedirectToAction("Login", "Access");

                    }
                    else
                    {
                        FlashMessage.Danger("Incorrect Current Password.");
                    }

                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }

        public ActionResult iCal()
        {
            List<CalendarEvent> events = db.EnrolSetting
              .Join(db.ScheduleClass, setting => setting.CompanyID, schedule => schedule.CompanyID,
                         (setting, schedule) => new { EnrollSetting = setting, ClassSchedule = schedule })

              .Join(db.Location, cls => cls.ClassSchedule.LocationID, loc => loc.LocationID,
                         (cls, loc) => new { clss = cls, locs = loc })

              .Join(db.CourseType, classes => classes.clss.ClassSchedule.CourseID, course => course.CourseTypeID,
                    (classes, course) => new { setting = classes.clss.EnrollSetting, Clases = classes.clss.ClassSchedule, course = course, location = classes.locs })

              .Where(st => st.setting.SiteName == SessionWrapper.Current.SubDomain && st.Clases.ScheduleDate >= DateTime.Now)
              .AsEnumerable().Select(st => new CalendarEvent { id = st.Clases.ClassID, IsShowRemainingSeatsLeft = st.course.CourseOptions_ShowNumberOfSeatsRemainingOnSchedulePage, CourseName = st.course.CourseName, ClassDate = st.Clases.ScheduleDate, ClassTime = st.Clases.StartTime, MaxStudents = st.Clases.MaxStudents, enrolledStudents = db.Student.Where(x => x.ClassID == st.Clases.ClassID).Count(), description = st.course.Description, allDay = false, start = st.Clases.ScheduleDate.Add(Convert.ToDateTime(st.Clases.StartTime).TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ssK"), url = st.Clases.ScheduleDate < DateTime.Today ? "" : string.Format("/ClassDetail?id={0}", st.Clases.ClassID), color = st.Clases.ScheduleDate < DateTime.Today ? "" : st.course.CalendarIconColor, textColor = "black", className = st.Clases.ScheduleDate < DateTime.Today ? "past-class" : "", ClassEndTime = st.Clases.EndTime, location = st.location.Name }).ToList();

            if (events!= null && events.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                string DateFormat = "yyyyMMddTHHmmssZ";
                string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
                sb.AppendLine("BEGIN:VCALENDAR");
                sb.AppendLine("PRODID:Enrollmart");
                sb.AppendLine("VERSION:2.0");
                sb.AppendLine("METHOD:PUBLISH");
                foreach (var res in events)
                {
                    DateTime dtStart = Convert.ToDateTime(res.ClassTime);
                    DateTime dtEnd = Convert.ToDateTime(res.ClassEndTime);
                    sb.AppendLine("BEGIN:VEVENT");
                    sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
                    sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
                    sb.AppendLine("DTSTAMP:" + now);
                    sb.AppendLine("UID:" + Guid.NewGuid());
                    sb.AppendLine("CREATED:" + now);
                    //sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + res.DetailsHTML);
                    sb.AppendLine("DESCRIPTION:" + res.enrolledStudents + " students registered");
                    sb.AppendLine("LAST-MODIFIED:" + now);
                    sb.AppendLine("LOCATION:" + res.location);
                    sb.AppendLine("SEQUENCE:0");
                    sb.AppendLine("STATUS:CONFIRMED");
                    sb.AppendLine("SUMMARY:" + res.CourseName);
                    sb.AppendLine("TRANSP:OPAQUE");
                    sb.AppendLine("END:VEVENT");
                }
                sb.AppendLine("END:VCALENDAR");
                var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(calendarBytes, "text/calendar", "event.ics");
            }
            else
            {
                return Content("No Class Schedule found");
            }

        }

        public ActionResult ActivityReport()
        {
            ReportFilters model = new ReportFilters();
            model.DateCriteria = "This Month";
            return View(model);
        }

        [HttpPost]
        public ActionResult ActivityReport(ReportFilters model)
        {
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult GetActivities(ReportFilters model)
        {

            var activities = GenerateActivities(model);
            return View(activities);
        }

        private IEnumerable<ClientActivityViewModel> GenerateActivities(ReportFilters model)
        {
            var activities = (from schedule in db.ScheduleClass
                         join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                         join discipline in db.Discipline on course.Discipline equals discipline.DisciplineID
                          where schedule.CompanyID == SessionWrapper.Current.CompanyID 
                         select new ClientActivityViewModel { CourseName = course.CourseName, ScheduleDate = schedule.ScheduleDate,  Discipline = discipline.DisciplineName, RegisteredStudents = db.Student.Where(x => x.ClassID == schedule.ClassID && x.IsUnSchedule == false).Count() });

            int month = 0, year = 0;
            if (model.DateCriteria == "This Month")
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
                activities = activities.Where(x => x.ScheduleDate.Month == month && x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Last Month")
            {
                DateTime dt = DateTime.Now.AddMonths(-1);
                month = dt.Month;
                year = dt.Year;
                activities = activities.Where(x => x.ScheduleDate.Month == month && x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "This Year")
            {
                year = DateTime.Today.Year;
                activities = activities.Where(x => x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Last Year")
            {
                DateTime dt = DateTime.Now.AddYears(-1);
                year = dt.Year;
                activities = activities.Where(x => x.ScheduleDate.Year == year);
            }
            else if (model.DateCriteria == "Custom Range")
            {
                activities = activities.Where(x => x.ScheduleDate >= model.DateForm.Date && x.ScheduleDate <= model.DateTo.Date);
            }
            return activities;

        }

        [ChildActionOnly]
        public ActionResult GetInstructorsByDiscipline(ReportFilters model)
        {

            var activities = GenerateInstructorsByDiscipline(model);
            return View(activities);
        }

        private dynamic GenerateInstructorsByDiscipline(ReportFilters model)
        {
            var activities = (from instructor in db.User
                              join certification in db.InstructorCertification on instructor.UserID equals certification.InstructorID
                              join discipline in db.Discipline on certification.DisciplineID equals discipline.DisciplineID
                              where instructor.CompanyID == SessionWrapper.Current.CompanyID && instructor.IsInstructor == true
                              select new { CreatedOn = certification.Initial, Discipline = discipline.DisciplineName }); 

            int month = 0, year = 0;
            if (model.DateCriteria == "This Month")
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
                activities = activities.Where(x => x.CreatedOn.Month == month && x.CreatedOn.Year == year);
            }
            else if (model.DateCriteria == "Last Month")
            {
                DateTime dt = DateTime.Now.AddMonths(-1);
                month = dt.Month;
                year = dt.Year;
                activities = activities.Where(x => x.CreatedOn.Month == month && x.CreatedOn.Year == year);
            }
            else if (model.DateCriteria == "This Year")
            {
                year = DateTime.Today.Year;
                activities = activities.Where(x => x.CreatedOn.Year == year);
            }
            else if (model.DateCriteria == "Last Year")
            {
                DateTime dt = DateTime.Now.AddYears(-1);
                year = dt.Year;
                activities = activities.Where(x => x.CreatedOn.Year == year);
            }
            else if (model.DateCriteria == "Custom Range")
            {
                activities = activities.Where(x => x.CreatedOn>= model.DateForm.Date && x.CreatedOn<= model.DateTo.Date);
            }

            var disciplines = activities.GroupBy(xp => new { xp.Discipline}).Select(xp => new { Discipline = xp.Key.Discipline, Instructors = xp.Count()}).ToList();
            //dynamic obj = new ExpandoObject();
            //obj.Disciplines = disciplines;
            return disciplines;

        }


        public ActionResult ClassReport()
        {
            ClassFiltersViewModel model = new ClassFiltersViewModel();
            model.DateCriteria = "This Month";
            return View(model);
        }

        [HttpPost]
        public ActionResult ClassReport(ClassFiltersViewModel model)
        {
            return View(model);
        }


        public ActionResult ProductAdonReport()
        {
            ReportFilters model = new ReportFilters();
            model.DateCriteria = "This Month";
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductAdonReport(ReportFilters model)
        {
            return View(model);
        }


        public ActionResult PaymentReport()
        {
            ReportFilters model = new ReportFilters();
            model.DateCriteria = "This Month";
            return View(model);
        }


        [HttpPost]
        public ActionResult PaymentReport(ReportFilters model)
        {
            return View(model);
        }

        public ActionResult PromocodeReport()
        {
            PromocodeReportFilters model = new PromocodeReportFilters();
            model.DateCriteria = "This Month";
            model.DateType = "Registration Dates";
            model.CodeID = 0;
            return View(model);
        }


        [HttpPost]
        public ActionResult PromocodeReport(PromocodeReportFilters model)
        {
            return View(model);
        }


        [HttpPost]
        public ActionResult AddPayment(StudentPayment model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.PaymentDate = DateTime.Now;
                    model.Amount = model.type == "Refund" ? ((-1) * model.Amount) : model.Amount;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.StudentPayment.Add(model);
                    int rowsAffected = db.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        {
                            FlashMessage.Confirmation("Payment Added Successfully");
                            return RedirectToAction("EditStudent", "Admin", new { id = model.StudentID });
                        }

                    }


                }
                catch (Exception ex)
                {
                    FlashMessage.Retrieve(ControllerContext.HttpContext);
                    FlashMessage.Danger(ex.Message);
                }

            }
            return View(model);
        }


        public ActionResult GetCustomDateRange()
        {
            ViewBag.Action = "UpcomingClasses";
            return PartialView("_DateRange");
        }

        #endregion

    }
}