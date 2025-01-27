using Abp.Runtime.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Companies.Dto
{
    public class CreateCompanyDto : ICustomValidate
    {
        [Required]
        public List<CompanyTranslationDto> Translations { get; set; }

        [Required]
        public int CityId { get; set; }


        // public CompanyContactDto CompanyContact { get; set; }
        //public List<CompanyBranchDto> CompanyBranches { get; set; }


        [Required]
        public long CompanyProfilePhotoId { get; set; }


        public List<long> Attachments { get; set; } = new List<long>();

        public SuperLiteUserDto userDto { get; set; }

        public JobType JobType { get; set; }



        public void AddValidationErrors(CustomValidationContext context)
        {
            if (CityId == 0)
                context.Results.Add(new ValidationResult("Region Id Cannot be 0"));



        }
    }
}
