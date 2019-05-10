using EnrolTraining.Common;
using EnrolTraining.Models;
using EnrolTraining.PaymentIntegration;
using PayPal.Api;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Vereyon.Web;

namespace EnrolTraining.Controllers
{
    public class EnrollController : Controller
    {

        private StripeCustomer GetCustomer()
        {
            var mycust = new StripeCustomerCreateOptions();
            mycust.SourceCard = new SourceCard()
            {
                Cvc = "786",
                Number = "4242424242424242",
                ExpirationMonth = 10,
                ExpirationYear = 2020,
                AddressCountry = "USA",
                AddressLine1 = "",
                AddressLine2 = "",
                AddressCity = "",
                AddressState = "",
                AddressZip = ""
            };
            mycust.Email = "friends_4ever8485@hotmail.com";
            mycust.Description = "Chris Moris";
            var customerservice = new StripeCustomerService("sk_test_2LVKznWm1WA4kvWngmGaPdLx");
            return customerservice.Create(mycust);
        }

        public ActionResult Calendar()
        {
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            Context db = new Context();
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            int locationID = 0;
            bool isDefaultLocationExist = db.Location.Any(x => x.CompanyID == companyID && x.IsDefaultSelectionForClasses == true);
            if (isDefaultLocationExist)
            {
                locationID = db.Location.Where(x => x.CompanyID == companyID && x.IsDefaultSelectionForClasses == true).Select(x => x.LocationID).FirstOrDefault();
            }
            else
            {
                locationID = db.Location.Where(x => x.CompanyID == companyID).Select(x => x.LocationID).FirstOrDefault();
            }

            ViewBag.LocationID = locationID;
            ViewBag.CompanyID = companyID;
            return View();
        }

        [HttpPost]
        public ActionResult Calendar(int locationID)
        {
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            Context db = new Context();
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            ViewBag.LocationID = locationID;
            ViewBag.CompanyID = companyID;
            return View();
        }

        public ActionResult DualCalendar()
        {
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            Context db = new Context();
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            int locationID = 0;
            bool isDefaultLocationExist = db.Location.Any(x => x.CompanyID == companyID && x.IsDefaultSelectionForClasses == true);
            if (isDefaultLocationExist)
            {
                locationID = db.Location.Where(x => x.CompanyID == companyID && x.IsDefaultSelectionForClasses == true).Select(x => x.LocationID).FirstOrDefault();
            }
            else
            {
                locationID = db.Location.Where(x => x.CompanyID == companyID).Select(x => x.LocationID).FirstOrDefault();
            }

            ViewBag.LocationID = locationID;
            ViewBag.CompanyID = companyID;
            return View();
        }


        [HttpPost]
        public ActionResult DualCalendar(int locationID)
        {
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            Context db = new Context();
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            ViewBag.LocationID = locationID;
            ViewBag.CompanyID = companyID;
            return View();
        }



        public ActionResult Schedule()
        {
            Context db = new Context();
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            ViewBag.CompanyID = companyID;
            ViewBag.LocationID = 0;
            var events = db.EnrolSetting
                          .Join(db.ScheduleClass, setting => setting.CompanyID, schedule => schedule.CompanyID,
                                     (setting, schedule) => new { EnrollSetting = setting, ClassSchedule = schedule })

                          .Join(db.Location, cls => cls.ClassSchedule.LocationID, loc => loc.LocationID,
                                     (cls, loc) => new { clss = cls, locs = loc})

                          .Join(db.CourseType, classes => classes.clss.ClassSchedule.CourseID, course => course.CourseTypeID,
                                (classes, course) => new { setting = classes.clss.EnrollSetting, Clases = classes.clss.ClassSchedule, course = course, location = classes.locs })

                          .Where(st => st.setting.SiteName == subdomain && st.Clases.ClientID == null && st.Clases.ScheduleDate >= DateTime.Now)
                          .AsEnumerable().Select(st => new CalendarEvent { id = st.Clases.ClassID, IsShowRemainingSeatsLeft = st.course.CourseOptions_ShowNumberOfSeatsRemainingOnSchedulePage, CourseName = st.course.CourseName, ClassDate = st.Clases.ScheduleDate, ClassTime = st.Clases.StartTime, MaxStudents = st.Clases.MaxStudents, enrolledStudents = db.Student.Where(x => x.ClassID == st.Clases.ClassID).Count(), description = st.course.Description, allDay = false, start = st.Clases.ScheduleDate.Add(Convert.ToDateTime(st.Clases.StartTime).TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ssK"), url = st.Clases.ScheduleDate < DateTime.Today ? "" : string.Format("/ClassDetail?id={0}", st.Clases.ClassID), color = st.Clases.ScheduleDate < DateTime.Today ? "" : st.course.CalendarIconColor, textColor = "black", className = st.Clases.ScheduleDate < DateTime.Today ? "past-class" : "", ClassEndTime = st.Clases.EndTime, location = st.location.Name, courseID = st.course.CourseTypeID }).ToList();
            return View(events);
        }


        [HttpPost]
        public ActionResult Schedule(int locationID)
        {
            Context db = new Context();
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            int companyID = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            ViewBag.LocationID = locationID;
            ViewBag.CompanyID = companyID;

            var events = db.EnrolSetting
                          .Join(db.ScheduleClass, setting => setting.CompanyID, schedule => schedule.CompanyID,
                                     (setting, schedule) => new { EnrollSetting = setting, ClassSchedule = schedule })

                          .Join(db.Location, cls => cls.ClassSchedule.LocationID, loc => loc.LocationID,
                                     (cls, loc) => new { clss = cls, locs = loc })

                          .Join(db.CourseType, classes => classes.clss.ClassSchedule.CourseID, course => course.CourseTypeID,
                                (classes, course) => new { setting = classes.clss.EnrollSetting, Clases = classes.clss.ClassSchedule, course = course, location = classes.locs })

                          .Where(st => st.location.LocationID == locationID && st.setting.SiteName == subdomain && st.Clases.ClientID == null && st.Clases.ScheduleDate >= DateTime.Now)
                          .AsEnumerable().Select(st => new CalendarEvent { id = st.Clases.ClassID, IsShowRemainingSeatsLeft = st.course.CourseOptions_ShowNumberOfSeatsRemainingOnSchedulePage, CourseName = st.course.CourseName, ClassDate = st.Clases.ScheduleDate, ClassTime = st.Clases.StartTime, MaxStudents = st.Clases.MaxStudents, enrolledStudents = db.Student.Where(x => x.ClassID == st.Clases.ClassID).Count(), description = st.course.Description, allDay = false, start = st.Clases.ScheduleDate.Add(Convert.ToDateTime(st.Clases.StartTime).TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ssK"), url = st.Clases.ScheduleDate < DateTime.Today ? "" : string.Format("/ClassDetail?id={0}", st.Clases.ClassID), color = st.Clases.ScheduleDate < DateTime.Today ? "" : st.course.CalendarIconColor, textColor = "black", className = st.Clases.ScheduleDate < DateTime.Today ? "past-class" : "", ClassEndTime = st.Clases.EndTime, location = st.location.Name, courseID = st.course.CourseTypeID }).ToList();
            return View(events);
        }

        public ActionResult ClassDetail(int id, int iid=0)
        {
            Context db = new Context();
            ClassDetailViewModel model = (from schedule in db.ScheduleClass 
                         join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                         join location in db.Location on schedule.LocationID equals location.LocationID into ps
                         from location in ps.DefaultIfEmpty()
                         where schedule.ClassID == id
                         select new ClassDetailViewModel {ClassID = schedule.ClassID, CourseImage = course.Image, CourseName = course.CourseName, CourseDescription = course.Description, Location = location == null ? "" : location.Name, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, CourseAddOns = course.AddOns, ExtraClasses = schedule.ExtraClassTimes }).FirstOrDefault() ;
            ViewBag.iid = iid;
            return View(model);
        }


        [HttpPost]
        public ActionResult StudentRegistration(ClassDetailViewModel model)
        {
            Context db = new Context();
            string options = string.Empty;
            string pickuppharase = string.Empty;
            bool showdeliveryoptions = false;
            bool showShippingInDeliveryOptions = false;
            int shippingDays = 0;
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            string paypalemail = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.PaypalEmail).FirstOrDefault();
            double optionsprice = 0;
            if (model.SelectedAddOns != null && model.SelectedAddOns.Length > 0)
            {
                options = String.Join(",", model.SelectedAddOns);
                int compannyid = 0;
                int[] addons = Array.ConvertAll(options.Split(','), int.Parse);
                IEnumerable<RegistrationAdOn> lstAddOns = db.RegistrationAdOn.Where(x => addons.Contains(x.AdOnID));
                compannyid = lstAddOns.ToList()[0].CompanyID;
                optionsprice = lstAddOns.Select(x => x.Price).Sum();
                showdeliveryoptions = lstAddOns.Any(x => x.Type == "Shippable");
                if (showdeliveryoptions)
                {
                    AdOnDeliveryOption delvroptions = db.AdOnDeliveryOption.Where(x => x.CompanyID == compannyid).FirstOrDefault();
                    if (delvroptions != null && !string.IsNullOrWhiteSpace(delvroptions.PickupText))
                    {
                        DateTime allowShippingTill = DateTime.Now.Date.AddDays(Convert.ToInt32(delvroptions.ShippingDays));
                        showShippingInDeliveryOptions =  allowShippingTill <= model.ScheduleDate.Date;
                        pickuppharase = delvroptions.PickupText;
                        shippingDays = delvroptions.ShippingDays;
                    }
                }
            }

            Student previous = new Student();

            if (model.iid > 0)
            {
                previous = db.Student.Where(x => x.StudentID == model.iid).FirstOrDefault();
            }


            //Attention: Answer Array needs to be adjust in student object

            Student student = (from schedule in db.ScheduleClass
                                          join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
                                          join location in db.Location on schedule.LocationID equals location.LocationID into ps
                                           from location in ps.DefaultIfEmpty()
                                           where schedule.ClassID == model.ClassID
                                          select new { ClassID = schedule.ClassID, CourseName = course.CourseName, Location = location == null ? "" :  location.Name, ClassPrice = schedule.Price, ScheduleDate = schedule.ScheduleDate, StartTime = schedule.StartTime, EndTime = schedule.EndTime, SelectedOptions = options, OptionsPrice = optionsprice, ShowDeliveryOptions = showdeliveryoptions, showShippingInDeliveryOptions = showShippingInDeliveryOptions, ShippingDays = shippingDays, PickupPharase = pickuppharase, ExtraClasstimes = schedule.ExtraClassTimes, FirstName = previous.FirstName, LastName = previous.LastName, Email = previous.Email, PrimaryPhone = previous.PrimaryPhone, AlternatePhone = previous.AlternatePhone, MailingAddress1 = previous.MailingAddress1, MailingAddress2 = previous.MailingAddress2, MailingCity = previous.MailingCity, MailingState = previous.MailingState, MailingZip = previous.MailingZip, IsBillingSameAsMailing = previous.IsBillingSameAsMailing, PayOnArrival = schedule.IsAllowedRegistrationWithoutPayment, ShippingCost = course.ShippingPrice, PaypalEmail = paypalemail, CourseType = course.Type, isPromptCertification = course.CourseOptions_PromptForCertificationDuringRegistration, KeycodeBankID = course.KeycodeBankID, locationID = location.LocationID})
                                          .AsEnumerable().Select(x=> new Student { ClassID = x.ClassID, CourseName = x.CourseName, Location = x.Location, ClassPrice = x.ClassPrice, ScheduleDate = x.ScheduleDate, StartTime = x.StartTime, EndTime = x.EndTime, SelectedOptions = options, OptionsPrice = optionsprice, ShowDeliveryOptions = x.ShowDeliveryOptions, showShippingInDeliveryOptions = x.showShippingInDeliveryOptions, ShippingDays = x.ShippingDays, PickupPharase = x.PickupPharase, ExtraClassTimes = x.ExtraClasstimes, FirstName = x.FirstName, LastName = x.LastName, Email = x.Email, PrimaryPhone = x.PrimaryPhone, AlternatePhone = x.AlternatePhone, MailingAddress1 = x.MailingAddress1, MailingAddress2 = x.MailingAddress2, MailingCity = x.MailingCity, MailingState = x.MailingState, MailingZip = x.MailingZip, IsBillingSameAsMailing = x.IsBillingSameAsMailing, PayOnArrival = x.PayOnArrival, ShippingCostMaterial = x.ShippingCost, PaypalEmail = x.PaypalEmail, CourseType = x.CourseType, IsPromptCertification = x.isPromptCertification, KeycodeBankID = x.KeycodeBankID, LocationID = x.locationID }).FirstOrDefault();
            return View(student);
        }

        [HttpPost]
        public ActionResult ShowEnrollmentReview(Student student)
        {
            Context db = new Context();
            double shippingCharges = 0;
            if (student.IsAcceptedPromoCode)
            {
                if (student.ShowDeliveryOptions)
                {
                    string materials = student.DeliveryOption;
                    if (materials == "Ship")
                    {
                        shippingCharges = Convert.ToDouble(student.ShippingCostMaterial);
                    }
                }

                var promo = db.PromoCode.Where(x => x.Code == student.PromoCode).FirstOrDefault();
                if (promo!= null)
                {
                    student.PromoCodeID = promo.CodeID;
                    if (promo.Type == "DollarsOff")
                    {
                        student.DiscountPrice = promo.DiscountValue;
                    }
                    else
                    {
                        double percent = promo.DiscountValue / 100;
                        if (promo.IsDiscountForShippingAndAdon)
                        {
                            double TotalPrice = student.ClassPrice + student.OptionsPrice + shippingCharges;
                            student.DiscountPrice = TotalPrice * percent;
                        }
                        else
                        {
                            student.DiscountPrice = student.ClassPrice * percent;
                        }
                    }
                }
            }
            return PartialView("_ReviewRegistration", student);
        }




        [ChildActionOnly]
        public ActionResult RegistrationRightPanel()
        {
            try
            {
                Context db = new Context();
                string subdomain = Request.Url.Host.Split(new char[] { '.' })[0]; //RouteData.Values["company"].ToString();
                Company oCompanny = (from setting in db.EnrolSetting
                                     join company in db.Company on setting.CompanyID equals company.CompanyID
                                     where setting.SiteName == subdomain
                                     select company).ToList()[0];

                return View(oCompanny);
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult GetScheduledClasses(int LocationID, DateTime start, DateTime end)
        {
            //past-class
            //<b>3:00pm</b> (8 left)<br />Heartsaver First Aid Skills Session (Parts 2 and 3)
            //var query = (from setting in db.EnrolSetting
            //             join company in db.Company on setting.CompanyID equals company.CompanyID
            //             join schedule in db.ScheduleClass on setting.CompanyID equals schedule.CompanyID
            //             join course in db.CourseType on schedule.CourseID equals course.CourseTypeID
            //             where setting.SiteName == subdomain && schedule.ScheduleDate >= start && schedule.ScheduleDate <= end
            //             select new Models.CalendarEvent { id = schedule.ClassID, title = course.CourseName, description = "click to register", allDay = true, start = schedule.ScheduleDate.Add(TimeSpan.Parse(schedule.StartTime)).ToString("yyyy-MM-ddTHH:mm:ssK"), url = Request.UrlReferrer.OriginalString, color = course.CalendarIconColor }).ToList() ;

            Context db = new Context();
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0]; //RouteData.Values["company"].ToString();
            //<b>3:00pm</b> (8 left)<br />Heartsaver First Aid Skills Session (Parts 2 and 3)
            var events = db.EnrolSetting
                          .Join(db.ScheduleClass, setting => setting.CompanyID, schedule => schedule.CompanyID,
                                     (setting, schedule) => new { EnrollSetting = setting, ClassSchedule = schedule })
                          .Join(db.CourseType,
                                classes => classes.ClassSchedule.CourseID,
                                course => course.CourseTypeID,
                                (classes, course) => new { setting = classes.EnrollSetting, Clases = classes.ClassSchedule, course = course })
                          .Where(st => st.setting.SiteName == subdomain && st.Clases.ClientID == null && st.Clases.LocationID == LocationID && st.Clases.ScheduleDate >= start && st.Clases.ScheduleDate <= end)
                          .AsEnumerable().Select(st => new CalendarEvent { id = st.Clases.ClassID, IsShowRemainingSeatsLeft = st.course.CourseOptions_ShowNumberOfSeatsRemainingOnSchedulePage,  CourseName = st.course.CourseName, ClassDate = st.Clases.ScheduleDate, ClassTime = st.Clases.StartTime, MaxStudents = st.Clases.MaxStudents, enrolledStudents = db.Student.Where(x => x.ClassID == st.Clases.ClassID).Count(), description = st.Clases.ScheduleDate < DateTime.Today ? "" : "click to register", allDay = false, start = st.Clases.ScheduleDate.Add(Convert.ToDateTime(st.Clases.StartTime).TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ssK"), url = st.Clases.ScheduleDate < DateTime.Today ? "" : string.Format("/ClassDetail?id={0}", st.Clases.ClassID) , color = st.Clases.ScheduleDate < DateTime.Today ? "" : st.course.CalendarIconColor, textColor = "black", className = st.Clases.ScheduleDate < DateTime.Today ? "past-class" : "" }).ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ValidateAndSetPromoCode(string PromoCode, int ClassID)
        {
            Context db = new Context();
            var course = db.ScheduleClass.Find(ClassID);
            int courseID = course.CourseID;
            int companyID = course.CompanyID;

            var promo = db.PromoCode.ToList().Where(x => x.CompanyID == companyID && x.NumOfUses > 0 && x.Code == PromoCode && DateTime.Now.Date >= x.StartDate.Date && DateTime.Now.Date <= x.EndDate.Date ).FirstOrDefault();
            if (promo != null)
            {
                if (promo.IsRestrictUseByCourseType)
                {
                    if (promo.CoursesAllowed.Split(',').ToList().Any(x => x == courseID.ToString()))
                    {
                        //Accepted
                        return Json(true);
                    }
                    else
                    {
                        //Not Accepted - Promocode cannot be applied to this course
                        return Json(false);
                    }
                }
                else
                {
                    //Accepted
                    return Json(true);
                }
            }
            else
            {
                //Not Accepted - Promocode not found or has been expired
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult CompleteRegistration(Student model)
        {
            Context db = new Context();
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //Student model = jss.Deserialize<Student>(st);
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            EnrollSetting seting = db.EnrolSetting.Where(x => x.SiteName == subdomain).FirstOrDefault();

            try
            {
                if (model.PaymentType == 2)
                {
                    var mycharge = new StripeChargeCreateOptions();
                    mycharge.Amount = (int)( (model.TotalClassPrice - model.DiscountPrice) * 100);
                    int paymentAmount = (int)((model.TotalClassPrice - model.DiscountPrice) * 100);
                    mycharge.Currency = "USD";
                    
                    mycharge.SourceCard = new SourceCard()
                    {
                        Number = model.CardNo,
                        ExpirationYear = Convert.ToInt32(model.ExpirationYear),
                        ExpirationMonth = Convert.ToInt32(model.ExpirationMonth),
                        AddressCountry = "US",
                        AddressLine1 = model.MailingAddress1,
                        AddressLine2 = model.MailingAddress2,
                        AddressCity = model.MailingCity,
                        AddressState = model.MailingState,
                        AddressZip = model.MailingZip,
                        Name = model.LastName + " " + model.FirstName + " (" + model.Email + ")",
                        Cvc = model.SecurityCode
                    };
                    mycharge.Description = "Payment received from student registration";
                    mycharge.Capture = true;
                    //mycharge.CustomerId = current.Id;
                    string key = seting.StripeLiveSecretKey; //"sk_test_2LVKznWm1WA4kvWngmGaPdLx";
                    var chargeservice = new StripeChargeService(key);
                    StripeCharge currentcharge = chargeservice.Create(mycharge);
                    //StripeCustomer current = GetCustomer();
                    if (currentcharge.Status == "succeeded" && currentcharge.Paid == true)
                    {
                        int StudentID = SaveRegistration(model);

                        StudentPayment payment = new StudentPayment();
                        payment.PaymentID = 0;
                        payment.PaymentDate = DateTime.Now;
                        payment.TransactionID = currentcharge.Id;
                        payment.type = "Credit Card";
                        payment.StudentID = StudentID;
                        payment.Detail = "";
                        payment.Amount = (model.TotalClassPrice - model.DiscountPrice);
                        db.StudentPayment.Add(payment);
                        db.SaveChanges();

                        Utilities.AssignKeycodesToCourseAddons(model.SelectedOptions, model.FirstName, model.LastName, model.Email, model.ClassID, StudentID);
                        Utilities.SendClassRegistrationConfirmationToStudent(model);

                        return RedirectToAction("RegistrationConfirmed", new { id = StudentID });
                    }
                    else
                    {
                        FlashMessage.Warning("Credit Card Transaction Fail");
                        return RedirectToAction("RegistrationError");
                    }
                }
                else
                {
                    int StudentID = SaveRegistration(model);
                    //SendRegistrationConfirmationToStudent(model);
                    return RedirectToAction("RegistrationConfirmed", new { id = StudentID });
                }
            }
            catch (Exception ex)
            {
                FlashMessage.Warning(ex.Message);
                return RedirectToAction("RegistrationError");
            }
        }


        [HttpPost]
        public ActionResult ProceedToPaypal(Student student)
        {
            try
            {
                string encodemodel = System.Web.Helpers.Json.Encode(student);
                string data = Uri.EscapeDataString(encodemodel);
                Context db = new Context();
                string subdomain = Request.Url.Host.Split(new char[] { '.' })[0]; 
                var oSetting = (from setting in db.EnrolSetting
                                          join company in db.Company on setting.CompanyID equals company.CompanyID
                                          where setting.SiteName == subdomain
                                          select new {companyid = setting.CompanyID, brand = company.CompanyName, domain = setting.SiteName, logo = setting.LogoUrl, webexperienceid = setting.WebExperienceID, payee = setting.PaypalEmail} ).FirstOrDefault();

                string WebExperienceProfileID = oSetting.webexperienceid;
                if (string.IsNullOrWhiteSpace(WebExperienceProfileID))
                {
                    WebExperienceProfileID = PaypalService.webexperience(oSetting.domain, oSetting.brand, oSetting.logo);
                    string query = "Update EnrollSettings Set WebExperienceID = '" + WebExperienceProfileID + "' Where CompanyID = " + oSetting.companyid;
                    var QueryResult = db.Database.ExecuteSqlCommand(query);
                }

                var payment = PaypalService.CreatePayment(GetBaseUrl(), "sale", (student.TotalClassPrice - student.DiscountPrice).ToString(), oSetting.payee, WebExperienceProfileID, data);
                string approvlurl = payment.GetApprovalUrl();
                return Redirect(approvlurl);
            }
            catch(Exception ex)
            {
                FlashMessage.Warning(ex.Message);
                return RedirectToAction("RegistrationError");
            }
        }


        public ActionResult OnAuthorizedPayment(string data, string paymentID, string payerID)
        {
            try
            {
                Context db = new Context();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Student model = jss.Deserialize<Student>(data);
                var payment = PaypalService.ExecutePayment(paymentID, payerID);
                if (payment.state == "approved")
                {
                    int StudentID = SaveRegistration(model);

                    StudentPayment pay = new StudentPayment();
                    pay.PaymentID = 0;
                    pay.PaymentDate = DateTime.Now.Date;
                    pay.TransactionID = payment.id;
                    pay.type = "Paypal";
                    pay.StudentID = StudentID;
                    pay.Detail = "";
                    pay.Amount = (model.TotalClassPrice - model.DiscountPrice);
                    db.StudentPayment.Add(pay);
                    db.SaveChanges();

                    Utilities.AssignKeycodesToCourseAddons(model.SelectedOptions, model.FirstName, model.LastName, model.Email, model.ClassID, StudentID);
                    Utilities.SendClassRegistrationConfirmationToStudent(model);

                    return RedirectToAction("RegistrationConfirmed", new { id = StudentID });
                }
                else
                {
                    FlashMessage.Warning("Paypal Transaction Fail");
                    return RedirectToAction("RegistrationError");
                }

            }
            catch (Exception ex)
            {
                FlashMessage.Warning(ex.Message);
                return RedirectToAction("RegistrationError");
            }
        }

        public ActionResult OnCancelPayment(string data)
        {
            FlashMessage.Warning("Payment is cancelled by the student");
            return RedirectToAction("RegistrationError");
        }

        public ActionResult RegistrationError()
        {
            return View();
        }
        
        public ActionResult RegistrationConfirmed(int id = 0)
        {
            ViewBag.subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            ViewBag.id = id;
            return View();
        }

        private int SaveRegistration(Student model)
        {
            Context db = new Context();
            string subdomain = Request.Url.Host.Split(new char[] { '.' })[0];
            int companyid = db.EnrolSetting.Where(x => x.SiteName == subdomain).Select(x => x.CompanyID).FirstOrDefault();
            model.CompanyID = companyid;
            if (model.QuestionID != null)
            {
                model.QuestionIDs = String.Join(",", model.QuestionID);
            }

            if (model.Answer != null)
            {
                string answers = "";
                for (int i = 0; i < model.Answer.Length; i++)
                {
                    if (i + 1 == model.Answer.Length)
                    {
                        answers = answers + model.Answer[i];
                    }
                    else
                    {
                        answers = answers + model.Answer[i] + Environment.NewLine;
                    }
                }
                model.Answers = answers;
            }


            if (model.KeycodeBankID > 0)
            {
                Keycode keycode = Utilities.RegisterKeycode(Convert.ToInt32(model.KeycodeBankID), model.FirstName, model.LastName, model.Email, model.ClassID);
                if (keycode != null && keycode.KeycodeID > 0)
                {
                    model.KeycodeID = keycode.KeycodeID;
                    model.Keycode = keycode.Key;
                }
            }

            if (model.IsAcceptedPromoCode == false)
            {
                model.PromoCode = "";
            }

            model.RegisterionDate = DateTime.Now;
            model.CompanyID = companyid;
            model.ReceiptCode = DateTime.Now.ToString("hhmmssMMyydd");
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Student.Add(model);
            int rowsAffected = db.SaveChanges();
            int studentid = model.StudentID;

            if (model.DiscountPrice > 0)
            {
                StudentPayment oDiscount = new StudentPayment();
                oDiscount.StudentID = studentid;
                oDiscount.type = "Discount";
                oDiscount.Detail = model.PromoCode;
                oDiscount.PaymentDate = DateTime.Now;
                oDiscount.Amount = model.DiscountPrice;
                db.StudentPayment.Add(oDiscount);
                db.SaveChanges();
            }

            return studentid;
        }

        private string GetBaseUrl()
        {
            return Request.Url.Scheme + "://" + Request.Url.Host;
        }


    }
}