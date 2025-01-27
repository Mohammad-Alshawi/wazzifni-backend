
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
            Advertisiment = 2,
            QR = 3,
            Category = 6,
            ContactUs = 8,
            City = 9,
            CompanyLogo = 10,
            CompanyImage = 11

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
            Doctorate = 4
        }

    }
}

