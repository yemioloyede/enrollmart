using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EnrolTraining.Common
{
    public class EnumModel
    {
        public enum CardType
        {
            [Description("2015 AHA Provider - 3 Card")]
            AHAProvider3 = 1,

            [Description("2015 AHA Provider - 2 Card")]
            AHAProvider2 = 2,

            [Description("2015 AHA Instructor - 3 Card")]
            AHAInstructor3 = 3,

            [Description("2015 AHA Instructor - 2 Card")]
            AHAInstructor2 = 4,

            [Description("2005 AHA")]
            AHA2005 = 5,

            [Description("ASHI 2010")]
            ASHI2010 = 6,

            [Description("ASHI 2015")]
            ASHI2015 = 7,

            [Description("ESCI - 6 card")]
            ESCI = 8,

            [Description("Avery Mailing Labels (5160)")]
            AveryLabels = 9,

            [Description("Avery Name Tags (5395)")]
            AveryTags = 10,
        }

        public enum CardOptions
        {
            [Description("Heartsaver First Aid CPR AED")]
            HSFACA = 1,

            [Description("Heartsaver CPR AED")]
            HSCA = 2,

            [Description("Heartsaver First Aid")]
            HSFA = 3,

            [Description("Heartsaver Pediatric First Aid (2010)")]
            HSPFA = 4,

            [Description("ASHI CPR, AED and Basic First Aid")]
            ACABAA = 5,

            [Description("ASHI CPR and AED")]
            ACA = 6,

            [Description("Heartsaver Pediatric First Aid CPR AED (2015")]
            HSPFACA = 7,
        }

        public enum CardAllignments
        {
            [Description("Up 1/4\"")]
            Up14 = 1,

            [Description("Up 3/16\"")]
            Up316 = 2,

            [Description("Up 1/8\"")]
            Up18 = 3,

            [Description("Up 1/16\"")]
            Up116 = 4,

            [Description("None")]
            None = 5,

            [Description("Down 1/16\"")]
            Down116 = 6,

            [Description("Down 1/8\"")]
            Down18 = 7,

            [Description("Down 3/16\"")]
            Down316 = 8,

            [Description("Down 1/4\"")]
            Down14 = 9,

        }

        public enum SchedulePageFormat
        {

            [Description("Calendar")]
            Calendar = 1,

            [Description("Dual Calendar")]
            DualCalendar = 2,

            [Description("Class List")]
            ClassList = 3,

        }

        public enum StudentManikinRatio
        {
            [Description("1:1")]
            OneOne = 1,

            [Description("2:1")]
            TwoOne = 2,

            [Description("3:1")]
            ThreeOne = 3,

            [Description("4:1")]
            FourOne = 4,

            [Description("5:1")]
            FiveOne = 5,

            [Description("6:1")]
            SixOne = 6,

            [Description("7:1")]
            SevenOne = 7,

            [Description("8:1")]
            EightOne = 8,

            [Description("9:1")]
            NineOne = 9,
        }

        public enum DisplayOrder
        {
            [Description("1")]
            Order1 = 1,

            [Description("2")]
            Order2 = 2,

            [Description("3")]
            Order3 = 3,

            [Description("4")]
            Order4 = 4,

            [Description("5")]
            Order5 = 5,

            [Description("6")]
            Order6 = 6,

            [Description("7")]
            Order7 = 7,

            [Description("8")]
            Order8 = 8,

            [Description("9")]
            Order9 = 9,

            [Description("10")]
            Order10 = 10,

            [Description("N/A")]
            NotOrder = 11,

        }

        public enum EnrollQuestionType
        {
            [Description("Text Box")]
            TextBox = 1,

            [Description("Drop Down")]
            DropDown = 2,

            //[Description("Radio Buttons - Vertical")]
            //RadioVertical = 3,

            //[Description("Radio Buttons - Horizontal")]
            //RadioHorizontal = 4,


        }

        public enum ClassIDNumbersGenerationMethod
        {

            [Description("Automatically Assigned")]
            Auto = 1,

            [Description("Not Used")]
            NotUsed = 2,

            [Description("Manually Assigned")]
            Manual = 3,
        }

        public enum StudentStatus
        {

            [Description("Pending")]
            Pending = 1,

            [Description("Complete")]
            Complete = 2,

            [Description("Incomplete")]
            Incomplete = 3,

            [Description("Remediate")]
            Remediate = 4,

            [Description("No Show")]
            NoShow = 5,

            [Description("Cancelled")]
            Cancelled = 6,
        }

        public enum EmailCampaignScheduleType
        {
            [Description("after")]
            after = 1,

            [Description("before")]
            before = 2,
        }

        public enum CourseDiscipline
        {
            [Description("Other")]
            Other = 1,

            [Description("ACLS")]
            ACLS = 2,

            [Description("BLS")]
            BLS = 3,

            [Description("PALS")]
            PALS = 4,

            [Description("HeartSaver")]
            HeartSaver = 5,

            [Description("EMS")]
            EMS = 6,

            [Description("NRP")]
            NRP = 7,

            [Description("PEARS")]
            PEARS = 8,

        }



    }
}