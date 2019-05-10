using EnrolTraining.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EnrolTraining.Common
{
    public static class Utilities
    {

        public static Keycode RegisterKeycode(int keycodeBankID, string FirstName, string LastName, string Email, int ClassID, int AddonID = 0)
        {
            Context db = new Context();
            Keycode keycode = db.Keycode.Where(x => x.KeyBankID == keycodeBankID && x.Registrant == null).FirstOrDefault();
            if (keycode != null && keycode.KeycodeID > 0)
            {
                keycode.Registrant = FirstName + " " + LastName;
                keycode.DateAssigned = DateTime.Now;
                keycode.AddonID = AddonID;
                keycode.Email = Email;
                db.Keycode.Attach(keycode);
                var entry = db.Entry(keycode);
                entry.State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return keycode;
        }

        public static List<string> GetCertificates(string path)
        {

            DirectoryInfo RootDirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            var list = new List<string>();
            if (Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                foreach (FileInfo fileInfo in RootDirInfo.GetFiles())
                {
                    list.Add(fileInfo.Name);
                }
            }
            return list;
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static string GetSMSEmail(string phone)
        {
            string smsEmail = "";
            using (WebClient webClient = new System.Net.WebClient())
            {
                NameValueCollection values = new NameValueCollection();
                values.Add("phonenum", phone);

                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] result = webClient.UploadValues("http://www.freesmsgateway.info", "POST", values);
                string ResultAuthTicket = Encoding.UTF8.GetString(result);
                string source = WebUtility.HtmlDecode(ResultAuthTicket);
                MatchCollection data = Regex.Matches(source, @"SMS Gateway Address: \s*(.+?)\s*<br>", RegexOptions.Singleline);
                foreach (Match m in data)
                {
                    smsEmail = m.Groups[1].Value;
                }
            }
            return smsEmail;
        }

        public static void SendEmail(string MailForm, string MailTo, string Subject, string Body, bool IsTextMessage)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("irfanibrahim1984@gmail.com", "###");

                    MailAddress from = new MailAddress(MailForm, MailForm);
                    MailAddress to = new MailAddress(MailTo);

                    MailMessage mail = new MailMessage(from, to);
                    mail.Subject = Subject;
                    if (IsTextMessage)
                    {
                        mail.Body = Body;
                        mail.IsBodyHtml = false;
                    }
                    else
                    {
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                    }
                    mail.ReplyToList.Add(MailForm);

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void SendClassRegistrationConfirmationToStudent(Student model)
        {
            Context db = new Context();
            var location = db.Location.Find(model.LocationID);

            var settings = (from setting in db.EnrolSetting
                            join company in db.Company on setting.CompanyID equals company.CompanyID
                            where setting.CompanyID == model.CompanyID
                            select new { CompnayEmail = company.Email, MailFrom = setting.MailForm }).FirstOrDefault();

            string body = "";
            string smsbody = "";

            smsbody = smsbody + "Dear " + model.FirstName + " " + model.LastName + ". ";
            smsbody = smsbody + "You have successfully been registered for " + model.CourseName + " scheduled on " + model.ScheduleDate.ToString("ddd M/d/yyyy") + " at " + model.StartTime + ".";
            smsbody = smsbody + "Check your email for detailed information";

            body = body + "Dear " + model.FirstName + " " + model.LastName + "<br /><br />";
            body = body + "Thank you for your registration for the following class:<br /><br />";



            body = body + "<table style=\"border-collapse:collapse;border:1px solid lightgray\"><tbody>";

            if (model.KeycodeBankID > 0)
            {
                string key = "(UnAssigned)";
                string instructions = "";
                if (model.KeycodeID > 0)
                {
                    var keycode = db.Keycode.Find(model.KeycodeID);
                    key = keycode.Key;
                    instructions = db.KeycodeBank.Find(keycode.KeyBankID).Instructions;
                }
                body = body + "<tr>";
                body = body + "<td style=\"vertical-align:top; border:1px solid lightgray\"><b>Class: </b></td>";
                body = body + "<td style=\"border:1px solid lightgray\">" + model.CourseName + "<br /> <b>Keycode: </b>" + key + (string.IsNullOrWhiteSpace(instructions) ? "" : "<br /><b>Instructions: </b>" + instructions) + "</td>";
                body = body + "</tr>";
            }
            else
            {
                body = body + "<tr>";
                body = body + "<td style=\"border:1px solid lightgray\"><b>Class:</b></td>";
                body = body + "<td style=\"border:1px solid lightgray\">" + model.CourseName + "</td>";
                body = body + "</tr>";

            }


            body = body + "<tr>";
            body = body + "<td style=\"border:1px solid lightgray\"><b>Class Date:</b></td>";
            body = body + "<td style=\"border:1px solid lightgray\">" + model.ScheduleDate.ToString("ddd M/d/yyyy") + " at " + model.StartTime + "</td>";
            body = body + "</tr>";



            List<CourseAdon> lstCourseAddons = db.CourseAdon.Where(x => x.StudentID == model.StudentID).OrderBy(x=>x.Type).ToList();
            if (lstCourseAddons != null && lstCourseAddons.Count > 0)
            {
                foreach (var addon in lstCourseAddons)
                {
                    body = body + "<tr>";
                    body = body + "<td style=\"vertical-align:top; border:1px solid lightgray\"><b>Add On:</b></td>";
                    if (addon.Type == "KeyCode")
                    {
                        body = body + "<td style=\"border:1px solid lightgray\">" + addon.AdOnName + "<br /> <b>Keycode: </b>" + (string.IsNullOrWhiteSpace(addon.Keycode) ? "(Unassigned)" : addon.Keycode) + (string.IsNullOrWhiteSpace(addon.Keycode) ? "" : "<br /><b>Instructions: </b>" + addon.KeycodeInstructions) + "</td>";
                    }
                    else
                    {
                        body = body + "<td style=\"border:1px solid lightgray\">" + addon.AdOnName + "</td>";
                    }
                    body = body + "</tr>";
                }
            }

            string url = "http://enrollmart.com/Access/Receipt?id=" +model.StudentID + "&code=" + model.ReceiptCode ;

            body = body + "<tr>";
            body = body + "<td style=\"border:1px solid lightgray\"><b>Receipt:</b></td>";
            body = body + "<td style=\"border:1px solid lightgray\"> <a target=\"_blank\" href=" + url + "> Click Here</td>";
            body = body + "</tr>";

            body = body + "<tr>";
            body = body + "<td style=\"border:1px solid lightgray\"><b>Location:</b></td>";
            body = body + "<td style=\"border:1px solid lightgray\">" + location.Name + "</td>";
            body = body + "</tr>";

            body = body + "<tr>";
            body = body + "<td style=\"border:1px solid lightgray\"><b>Directions:</b></td>";
            body = body + "<td style=\"border:1px solid lightgray\">" + location.Directions + "</td>";
            body = body + "</tr>";

            body = body + "</tbody></table>";
            body = body + "<br /><br /> THANK YOU";

            string subject = "Your Training Confirmation";

            SendEmail(string.IsNullOrWhiteSpace(settings.MailFrom) ? settings.CompnayEmail : settings.MailFrom, model.Email, subject, body, false);

            string smsEmail = GetSMSEmail(model.PrimaryPhone);
            if (!string.IsNullOrWhiteSpace(smsEmail))
            {
                SendEmail(string.IsNullOrWhiteSpace(settings.MailFrom) ? settings.CompnayEmail : settings.MailFrom, smsEmail, subject, smsbody, true);
            }

        }

        public static void AssignKeycodesToCourseAddons(string SelectedOptions, string FirstName, string LastName, string Email, int ClassID, int StudentID)
        {
            Context db = new Context();
            if (!string.IsNullOrWhiteSpace(SelectedOptions))
            {
                int[] addons = Array.ConvertAll(SelectedOptions.Split(','), int.Parse);
                List<RegistrationAdOn> lstAddOns = db.RegistrationAdOn.Where(x => addons.Contains(x.AdOnID)).ToList();
                foreach (var addon in lstAddOns)
                {

                    CourseAdon courseAdon = new CourseAdon();
                    courseAdon.AdonID = addon.AdOnID;
                    courseAdon.Price = addon.Price;
                    courseAdon.Type = addon.Type;
                    courseAdon.AdOnName = addon.Name;
                    courseAdon.StudentID = StudentID;
                    courseAdon.RegisterDate = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.CourseAdon.Add(courseAdon);
                    db.SaveChanges();
                    int courseAdonID = courseAdon.ID;

                    if (addon.Type == "KeyCode")
                    {
                        Keycode keycode = Common.Utilities.RegisterKeycode(Convert.ToInt32(addon.KeycodeID), FirstName, LastName, Email, ClassID, addon.AdOnID);
                        if (keycode != null && keycode.KeycodeID > 0)
                        {


                            string instructions = db.KeycodeBank.Find(keycode.KeyBankID).Instructions;

                            db.Configuration.ValidateOnSaveEnabled = false;
                            var createdAdon = db.CourseAdon.Find(courseAdonID);

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

                    }
                }
            }
        }


        public static void UpdadteCourseAdons(string SelectedOptions, int StudentID)
        {
            Context db = new Context();
            //var QueryResult = db.Database.ExecuteSqlCommand("Delete from CourseAdons Where StudentID = " + StudentID);

            if (!string.IsNullOrWhiteSpace(SelectedOptions))
            {
                int[] addons = Array.ConvertAll(SelectedOptions.Split(','), int.Parse);
                List<RegistrationAdOn> lstSelectedAddon = db.RegistrationAdOn.Where(x => addons.Contains(x.AdOnID)).ToList();
                List<CourseAdon> lstCourseAdons = db.CourseAdon.Where(x => x.StudentID == StudentID).ToList();

                //CHECK IF NOT EXIST NOW AND EXIST IN COURSE ADDON THEN DELETE FROM COURSE ADDON 
                //CHECK IF EXIST NOW AND NOT EXIST IN COURSE ADDON THEN ADD TO COURSE ADD ON

                foreach (var selectedAddon in lstSelectedAddon)
                {

                    if (lstCourseAdons.Any(x=>x.AdonID == selectedAddon.AdOnID) == false)
                    {
                        CourseAdon courseAdon = new CourseAdon();
                        courseAdon.AdonID = selectedAddon.AdOnID;
                        courseAdon.AdOnName = selectedAddon.Name;
                        courseAdon.Price = selectedAddon.Price;
                        courseAdon.Type = selectedAddon.Type;
                        courseAdon.StudentID = StudentID;
                        courseAdon.RegisterDate = DateTime.Now;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.CourseAdon.Add(courseAdon);
                        db.SaveChanges();
                        lstCourseAdons.Add(courseAdon);
                    }
                }




                foreach (var courseAddon in lstCourseAdons)
                {
                    if (lstSelectedAddon.Any(x => x.AdOnID == courseAddon.AdonID) == false)
                    {
                        db.CourseAdon.Attach(courseAddon);
                        var entity = db.Entry(courseAddon);
                        entity.State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void AddStudentDiscount(PromoCode promo, int StudentID, double ClassPrice, double TotalPrice)
        {
            Context db = new Context();

            StudentPayment oDiscount = new StudentPayment();
            oDiscount.StudentID = StudentID;
            oDiscount.type = "Discount";
            oDiscount.Detail = promo.Code;
            oDiscount.PaymentDate = DateTime.Now;

            if (promo.Type == "DollarsOff")
            {
                oDiscount.Amount = promo.DiscountValue;
            }
            else
            {
                double percent = promo.DiscountValue / 100;
                if (promo.IsDiscountForShippingAndAdon)
                {
                    oDiscount.Amount = TotalPrice * percent;
                }
                else
                {
                    oDiscount.Amount = ClassPrice * percent;
                }
            }
            db.StudentPayment.Add(oDiscount);
            db.SaveChanges();
        }

        public static void DeleteStudentDiscount(StudentPayment discount)
        {
            Context db = new Context();

            db.StudentPayment.Attach(discount);
            var entity = db.Entry(discount);
            entity.State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

        }


        public static void ApplyPromoCode(int StudentID, int PromoCodeID, double ClassPrice, double TotalClassPrice)
        {
            Context db = new Context();

            var discount = db.StudentPayment.Where(x => x.StudentID == StudentID && x.type == "Discount").FirstOrDefault();
            if (PromoCodeID > 0)
            {
                var promo = db.PromoCode.Find(PromoCodeID);

                if (discount == null)
                {
                    AddStudentDiscount(promo, StudentID, ClassPrice, TotalClassPrice);
                }
                else
                {
                    if (discount.Detail != promo.Code)
                    {
                        DeleteStudentDiscount(discount);
                        AddStudentDiscount(promo, StudentID, ClassPrice, TotalClassPrice);
                    }
                }
            }
            else
            {
                if (discount != null)
                {
                    DeleteStudentDiscount(discount);
                }
            }

        }

    }
}