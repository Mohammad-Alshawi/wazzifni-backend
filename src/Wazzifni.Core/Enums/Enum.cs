
using System.ComponentModel.DataAnnotations;
using Wazzifni.Localization.SourceFiles;


namespace Wazzifni.Enums
{
    public class Enum
    {


        public enum ConfirmationCodeType : byte
        {
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.ForgetPassword))]
            ForgotPassword = 1,
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.ConfirmPhoneNumber))]
            ConfirmPhoneNumber = 2,
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.ConfirmEmail))]
            ConfirmEmail = 3,

        }

        public enum AttachmentRefType : byte
        {
            Profile = 1,
            CompanyLogo = 2,
            CompanyImage = 3,
            City = 4,
            SpokenLanguage = 5,
            CV = 6,
            Advertisiment = 7,
            Trainee = 8,
            CourseCategory = 9,
            Teacher = 10,
            Course = 11,

        }
        public enum AttachmentType : byte
        {
            PDF = 1,
            WORD = 2,
            JPEG = 3,
            PNG = 4,
            JPG = 5,
            MP4 = 6,
            MP3 = 7,
            APPLICATION = 8,
            HEIC = 9,
            HEIF = 10,

        }
        public enum ImageType : byte
        {
            JPEG = 1,
            PNG = 2,
            JPG = 3,
            HEIC = 4,
            HEIF = 5,

        }
        public enum VideoType : byte
        {
            MPEG = 1,
            OGG = 2,
            MP4 = 3,
            MOV = 4,
            AVI = 5,
            MPG = 6,
            WMV = 7,
            MKV = 8,
            M4V = 9,
        }


        public enum UserType : byte
        {
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.Admin))]
            Admin = 1,
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.BasicUser))]
            BasicUser = 2,
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.CompanyUser))]
            CompanyUser = 3,
            [Display(ResourceType = typeof(Tokens), Name = nameof(Tokens.Trainee))]
            Trainee = 4,


        }


        public enum DataType : byte
        {
            Boolean = 1,
            Integer = 2,
            String = 3,
            Double = 4
        }



        public enum PositionForAdvertisiment : byte
        {
            Top = 1,
            InBetween = 2
        }
        public enum Screen : byte
        {
            Home = 1,
            Properties = 2,
            Vehicles = 3,
            Wanted = 4
        }
        public enum ContactUsRequestType : byte
        {
            Complaint = 1,
            Suggestion = 2
        }
        public enum ContactUsRequestStatues : byte
        {
            Checking = 1,
            Followed = 2
        }

        public enum AppType : byte
        {
            Apk = 1,
            Ios = 2
        }



        public enum SystemType : byte
        {
            Android = 1,
            Ios = 2,
        }
        public enum AppTypes : byte
        {
            Basic = 1,
            Business = 2,
            Both = 3
        }
        public enum UpdateOptions : byte
        {
            Optional = 1,
            Mandatory = 2,
            Nothing = 3
        }

        public enum CompanyStatus : byte
        {
            Checking = 1,
            Approved = 2,
            Rejected = 3
        }
        public enum JobType : byte
        {
            SoftwareDeveloper = 1,
            DataScientist = 2,
            ProductManager = 3,
            GraphicDesigner = 4,
            SystemAdministrator = 5,
            SalesRepresentative = 6,
            DigitalMarketingSpecialist = 7,
            ContentWriter = 8,
            CustomerSupport = 9,
            HumanResourcesManager = 10,
            FinancialAnalyst = 11,
            MechanicalEngineer = 12,
            ElectricalEngineer = 13,
            CivilEngineer = 14
        }


        public enum EducationLevel : byte
        {
            HighSchool = 1,
            Bachelor = 2,
            Master = 3,
            Doctorate = 4,
            MiddleSchool = 5
        }

        public enum SpokenLanguageLevel : byte
        {
            Beginner = 1,
            Elementary = 2,
            Intermediate = 3,
            UpperIntermediate = 4,
            Advanced = 5,
            Fluent = 6
        }

        public enum WorkEngagement : byte
        {
            FullTime = 1,
            PartTime = 2,
            Contract = 3,
            ProjectOnly = 4,
            StrategicPartnership = 5
        }


        public enum WorkPlace : byte
        {
            OnSite = 1,
            HalfRemote = 2,
            Remote = 3,

        }

        public enum WorkLevel : byte
        {
            TeamLeader = 1,
            Professional = 2,
            Beginner = 3,
            Manager = 4
        }

        public enum WorkPostStatus : byte
        {
            Checking = 1,
            Approved = 2,
            Rejected = 3
        }


        public enum WorkAvailbility : byte
        {
            Available = 1,
            Unavilable = 2
        }

        public enum WorkApplicationStatus : byte
        {
            CheckingByAdmin = 1,
            Approved = 2,
            RejectedByCompany = 3,
            CheckingByCompany = 4,
            RejectedByAdmin = 5

        }

        public enum FilterOnTime : byte
        {
            ThisToday = 1,
            ThisWeek = 2,
            ThisMonth = 3
        }

        public enum TopicType : byte
        {
            All = 0,
            Admin = 1,
            BasicUser = 2,
            CompanyUser = 3,
            Trainee = 4

        }

        public enum CourseMode : byte
        {
            Online = 1,
            InPerson = 2
        }

        public enum CourseDifficulty : byte
        {
            Beginner = 1,
            Intermediate = 2,
            Advanced = 3
        }

        public enum Gender : byte 
        { 
          Male = 1,
          Female = 2
        }

        public enum CourseRegistrationRequestStatus : byte 
        {
            Checking = 1,
            Approved = 2,
            Rejected = 3
        }

    }
}

