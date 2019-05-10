using EnrolTraining.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EnrolTraining
{
    public static class DropdownLists
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            var output = new StringBuilder();
            output.Append(@"<div class=""checkboxList"">");

            foreach (var item in items)
            {
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");
                if (item.Selected)
                    output.Append(@" checked=""chekced""");
                output.Append(" />");
                output.Append(item.Text);
                output.Append("<br />");
            }
            output.Append("</div>");

            return new MvcHtmlString(output.ToString()) ;
        }

        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        private static Hashtable GetEnumToBind(Type enumeration)
        {
            String[] names = Enum.GetNames(enumeration);
            Array values = Enum.GetValues(enumeration);
            Hashtable ht = new Hashtable();
            for (int i = 0; i < names.Length; i++)
            {
                MemberInfo[] memInfo = enumeration.GetMember(names[i]);
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    ht.Add(Convert.ToInt32(values.GetValue(i)), ((DescriptionAttribute)attrs[0]).Description);
                }
            }
            return ht;
        }

        private static List<SelectListItem> BindEnumList(Hashtable hashtable, string selectedValue, bool isFirstEmptyItem, string EmptyString)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            if (isFirstEmptyItem)
            {
                SelectListItem en1 = new SelectListItem();
                en1.Value = "";
                en1.Text = EmptyString;
                lst.Add(en1);
            }
            Dictionary<object, object>  r = hashtable.Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value);
            var l = r.OrderBy(key => key.Key);
            var dic = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

            foreach (var entry in dic)
            {
                SelectListItem en = new SelectListItem();
                en.Value = Convert.ToString(entry.Key);
                en.Text = Convert.ToString(entry.Value);
                if (en.Value == selectedValue)
                {
                    en.Selected = true;
                }
                lst.Add(en);
            }

            return lst;
        }

        public static Dictionary<string, string> USAStates()
        {
            Dictionary<string, string> states = new Dictionary<string, string>();
            states.Add("AL", "Alabama");
            states.Add("AK", "Alaska");
            states.Add("AZ", "Arizona");
            states.Add("AR", "Arkansas");
            states.Add("CA", "California");
            states.Add("CO", "Colorado");
            states.Add("CT", "Connecticut");
            states.Add("DE", "Delaware");
            states.Add("FL", "Florida");
            states.Add("GA", "Georgia");
            states.Add("HI", "Hawaii");
            states.Add("ID", "Idaho");
            states.Add("IL", "Illinois");
            states.Add("IN", "Indiana");
            states.Add("IA", "Iowa");
            states.Add("KS", "Kansas");
            states.Add("KY", "Kentucky");
            states.Add("LA", "Louisiana");
            states.Add("ME", "Maine");
            states.Add("MD", "Maryland");
            states.Add("MA", "Massachusetts");
            states.Add("MI", "Michigan");
            states.Add("MN", "Minnesota");
            states.Add("MS", "Mississippi");
            states.Add("MO", "Missouri");
            states.Add("MT", "Montana");
            states.Add("NE", "Nebraska");
            states.Add("NV", "Nevada");
            states.Add("NH", "New Hampshire");
            states.Add("NJ", "New Jersey");
            states.Add("NM", "New Mexico");
            states.Add("NY", "New York");
            states.Add("NC", "North Carolina");
            states.Add("ND", "North Dakota");
            states.Add("OH", "Ohio");
            states.Add("OK", "Oklahoma");
            states.Add("OR", "Oregon");
            states.Add("PA", "Pennsylvania");
            states.Add("RI", "Rhode Island");
            states.Add("SC", "South Carolina");
            states.Add("SD", "South Dakota");
            states.Add("TN", "Tennessee");
            states.Add("TX", "Texas");
            states.Add("UT", "Utah");
            states.Add("VT", "Vermont");
            states.Add("VA", "Virginia");
            states.Add("WA", "Washington");
            states.Add("WV", "West Virginia");
            states.Add("WI", "Wisconsin");
            states.Add("WY", "Wyoming");

            return states;
        }


        //not in use- getting from database
        public static List<SelectListItem> CourseDisciplines(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.CourseDiscipline));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> CardOptions(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.CardOptions));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> StudentStatus(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.StudentStatus));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> DisplayOrder(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.DisplayOrder));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> CardTypes(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.CardType));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> SchedulePageFormat(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.SchedulePageFormat));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> CardAllignments(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.CardAllignments));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> StudentManikinRatio(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.StudentManikinRatio));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> EmailCampaignScheduleType(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.EmailCampaignScheduleType));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> ClassIDNumbersGenerationMethod(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.ClassIDNumbersGenerationMethod));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> EnrollQuestionType(string selectedValue, bool isFirstEmptyItem, string EmptyString = "Please Select")
        {
            Hashtable result = GetEnumToBind(typeof(EnumModel.EnrollQuestionType));
            return BindEnumList(result, selectedValue, isFirstEmptyItem, EmptyString);
        }

        public static List<SelectListItem> RegistrationAdOns(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            IEnumerable<Models.RegistrationAdOn> Items = db.RegistrationAdOn.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.AdOnID.ToString();
                sl.Text = item.Name;
                if (SelectedValue != null)
                {
                    if (SelectedValue.Split(',').Contains(item.AdOnID.ToString()))
                    //if (sl.Value == SelectedValue.Split(',').ToList().Where(x => x.Contains(item.AdOnID.ToString())).FirstOrDefault())
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }


        public static List<SelectListItem> ClassAddons(string CourseAddOns, string selectedoptions="")
        {
            Context db = new Context();
            int[] addons = Array.ConvertAll(CourseAddOns.Split(','), int.Parse);
            List <SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.RegistrationAdOn> options = db.RegistrationAdOn.Where( x=> addons.Contains(x.AdOnID));

            foreach (var item in options)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.AdOnID.ToString();
                sl.Text = item.Name + " ($" + item.Price+ ")";

                bool isOptionSelected = selectedoptions.Split(',').Contains(item.AdOnID.ToString());

                if (isOptionSelected)
                {
                    sl.Selected = true;
                }

                if (item.Type == "KeyCode" && isOptionSelected == true)
                {
                    sl.Disabled = true;
                }


                lst.Add(sl);

            }
            return lst;
        }
        public static List<SelectListItem> Disciplines(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.Discipline> Items = db.Discipline.Where(x=> x.CompanyID == 0 || x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.DisciplineID.ToString();
                sl.Text = item.DisciplineName;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> ClassesDetails(string subdomain, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            var settings = (from setting in db.EnrolSetting
                            join course in db.CourseType on setting.CompanyID equals course.CompanyID
                            join cls in db.ScheduleClass on course.CourseTypeID equals cls.CourseID
                            where setting.SiteName == subdomain && cls.ScheduleDate >= DateTime.Now
                            select new {ClassID = cls.ClassID, CourseName = course.CourseName, ClassDate = cls.ScheduleDate, StartTime = cls.StartTime, EndTime = cls.EndTime });


            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in settings)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ClassID.ToString();
                sl.Text = item.CourseName + " (" + item.ClassDate.ToString("dd-MMM-yyyy") + " at " + item.StartTime + ")";
                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Locations(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.Location> Items = db.Location.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.IsArchive == false);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.LocationID.ToString();
                sl.Text = item.Name;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> LocationsOnCalendar(string SelectedValue, int CompanyID)
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.Location> Items = db.Location.Where(x => x.CompanyID == CompanyID && x.IsArchive == false);

            if (SelectedValue == "0")
            {
                SelectListItem s = new SelectListItem();
                s.Text = "All Locations";
                s.Value = "0";
                lst.Add(s);

            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.LocationID.ToString();
                sl.Text = item.Name;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Clients(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.Client> Items = db.Client.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ClientID.ToString();
                sl.Text = item.ClientCompany;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Templates(bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> lstCertificates = Common.Utilities.GetCertificates("/uploads/" + SessionWrapper.Current.SubDomain + "/Certificates/");
            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in lstCertificates)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ToString();
                sl.Text = item.ToString();
                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Rescheduling(int classid)
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            int currentMonth = DateTime.Now.Month;
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);
            int crsid = db.ScheduleClass.Where(x => x.ClassID == classid).Select(x => x.CourseID).FirstOrDefault();
            IEnumerable<Models.ScheduleClass> Items = db.ScheduleClass.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.CourseID == crsid && (x.ScheduleDate >= startDate && x.ScheduleDate <= endDate));

            SelectListItem empty = new SelectListItem();
            empty.Text = "Select new date";
            empty.Value = "";
            lst.Add(empty);

            //SelectListItem createClass = new SelectListItem();
            //createClass.Text = "Create a new class for this student";
            //createClass.Value = "n";
            //lst.Add(createClass);

            SelectListItem unscheduleClass = new SelectListItem();
            unscheduleClass.Text = "UnScheduled";
            unscheduleClass.Value = "u";
            lst.Add(unscheduleClass);

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ClassID.ToString();
                sl.Text = (DateTime.Now.Date > item.ScheduleDate ? "PAST " : "") +  item.ScheduleDate.Add(Convert.ToDateTime(item.StartTime).TimeOfDay).ToString("dddd, MMM dd, yyyy 'at' h:mm tt");
                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> DeliveryType(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Pickup", "Ship" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> PromocodeDateTypes(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Registration Dates", "Class Start Dates" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> PaymentTypes(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Credit Card", "Debit Card", "Cash", "Check", "Paypal", "Adjustment", "Refund" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }
        public static List<SelectListItem> StudentSearchFilters(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Name", "Email", "Phone" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> ClassDateFilter(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Next 30 Days", "Next 90 Days", "Custom Range" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> ClientDatesFilter(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "This Month", "Last Month", "This Year", "Last Year", "Custom Range" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }


        public static List<SelectListItem> PastClassDateFilter(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Last 30 Days", "Last 90 Days", "All Dates" , "Custom Range" };

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }


        public static List<SelectListItem> KeycodeSaleFilters(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<string> Items = new List<string> { "Last 12 Months", "Last 24 Months", "All"};

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item;
                sl.Text = item;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }


        public static List<SelectListItem> CoursTypes(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.CourseType> Items = db.CourseType.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.IsArchive == false);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.CourseTypeID.ToString();
                sl.Text = item.CourseName;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Instructors(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.User> Items = db.User.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.IsActive == true && x.IsInstructor == true);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.UserID.ToString();
                sl.Text = item.FirstName + " " + item.LastName;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Assistants(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();
            IEnumerable<Models.User> Items = db.User.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.IsActive == true && x.IsInstructorAssistant == true);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.UserID.ToString();
                sl.Text = item.FirstName + " " + item.LastName;
                if (SelectedValue != null)
                {
                    if (sl.Value == SelectedValue)
                    {
                        sl.Selected = true;
                    }
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> CardSettingProfiles(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select", bool ShowNewProfileAtEnd = true)
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            IEnumerable<Models.CardSetting> Items = db.CardSetting.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ProfileID.ToString();
                sl.Text = item.ProfileName;
                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }
                lst.Add(sl);
            }
            if (ShowNewProfileAtEnd)
            {
                SelectListItem slct = new SelectListItem();
                slct.Value = "0";
                slct.Text = "-- New Profile --";
                lst.Add(slct);

            }


            return lst;
        }

        public static List<SelectListItem> CardPrintingProfiles()
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            IEnumerable<Models.CardSetting> Items = db.CardSetting.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);


            foreach (var item in Items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.ProfileID.ToString();
                sl.Text = item.ProfileName;
                if (item.IsDefault)
                {
                    sl.Selected = true;
                }
                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> KeycodeBanks(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            IEnumerable<Models.KeycodeBank> items = db.KeycodeBank.Where(x=> x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.KeycodeBankID.ToString();
                sl.Text = item.Name;
                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> Taxes(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            IEnumerable<Models.Tax> items = db.Tax.Where(x => x.CompanyID == SessionWrapper.Current.CompanyID);

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in items)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.TaxID.ToString();
                sl.Text = item.Description + "(" + item.Percentage.ToString() + "%)";
                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> PromoCodes(int CourseID, string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            List<Models.PromoCode> items = db.PromoCode.ToList().Where(x => x.CompanyID == SessionWrapper.Current.CompanyID && x.NumOfUses > 0 && DateTime.Now.Date  >= x.StartDate.Date && DateTime.Now.Date <= x.EndDate.Date).ToList();

            int selectedPromoCodeID = Convert.ToInt32(SelectedValue);
            if (selectedPromoCodeID > 0)
            {
                bool isExist = items.Any(x => x.CodeID == selectedPromoCodeID);
                if (isExist == false)
                {
                    var promoCode = db.PromoCode.Find(selectedPromoCodeID);
                    items.Add(promoCode);
                }
            }

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (var item in items)
            {
                SelectListItem sl = new SelectListItem();

                if (item.IsRestrictUseByCourseType)
                {
                    if (item.CoursesAllowed.Split(',').ToList().Any(x=> x == CourseID.ToString()))
                    {
                        sl.Value = item.CodeID.ToString();
                        sl.Text = item.Code;
                    }
                }
                else
                {
                    sl.Value = item.CodeID.ToString();
                    sl.Text = item.Code;
                }

                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }


        public static List<SelectListItem> AllPromoCodes(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            Context db = new Context();
            List<SelectListItem> lst = new List<SelectListItem>();

            List<Models.PromoCode> items = db.PromoCode.ToList().Where(x => x.CompanyID == SessionWrapper.Current.CompanyID).ToList();

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "0";
                lst.Add(s);
            }

            foreach (var item in items)
            {
                SelectListItem sl = new SelectListItem();

                    sl.Value = item.CodeID.ToString();
                    sl.Text = item.Code;

                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> TimeZones(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {
            DateTimeFormatInfo dateFormats = CultureInfo.CurrentCulture.DateTimeFormat;
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            List<SelectListItem> lst = new List<SelectListItem>();

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            foreach (TimeZoneInfo timeZone in timeZones)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = timeZone.Id;
                sl.Text = timeZone.DisplayName;
                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }

        public static List<SelectListItem> SelectUSAStates(string SelectedValue, bool isFirstEmpty, string EmptyString = "Please Select")
        {

            List<SelectListItem> lst = new List<SelectListItem>();
            Dictionary<string, string> states = USAStates();

            if (isFirstEmpty)
            {
                SelectListItem s = new SelectListItem();
                s.Text = EmptyString;
                s.Value = "";
                lst.Add(s);
            }

            

            foreach (var item in states)
            {
                SelectListItem sl = new SelectListItem();
                sl.Value = item.Key;
                sl.Text = item.Value;
                if (sl.Value == SelectedValue)
                {
                    sl.Selected = true;
                }

                lst.Add(sl);
            }
            return lst;
        }
    }
}