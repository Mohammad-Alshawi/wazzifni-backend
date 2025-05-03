using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.CourseRegistrationRequests.Dto
{
    public class CreateCourseRegistrationRequestDto : ICustomValidate
    {
        public int CourseId { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (CourseId < 1)
                context.Results.Add(new ValidationResult("CourseId is required"));
        }
    }
}
