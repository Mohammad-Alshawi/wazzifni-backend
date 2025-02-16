using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.WorkApplications.Dto
{
    public class CreateWorkApplicationDto : ICustomValidate
    {
        public long WorkPostId { get; set; }
        public string Description { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (WorkPostId < 1)
                context.Results.Add(new ValidationResult("WorkPostId is required"));
        }
    }
}
