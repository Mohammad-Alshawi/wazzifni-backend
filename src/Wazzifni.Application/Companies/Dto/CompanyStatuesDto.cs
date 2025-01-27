using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Companies.Dto
{
    public class CompanyStatusDto : ICustomValidate
    {
        public int CompanyId { get; set; }
        public CompanyStatus Statues { get; set; }
        public string ReasonRefuse { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if ((Statues == CompanyStatus.Rejected || Statues == CompanyStatus.Rejected) && string.IsNullOrEmpty(ReasonRefuse))
                context.Results.Add(new ValidationResult("ReasonRefuse is Requierd"));

        }
    }


}
