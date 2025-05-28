using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;

namespace Wazzifni.CourseRegistrationRequests.Dto
{
    public class CreateCourseRegistrationRequestDto : ICustomValidate
    {
        public int CourseId { get; set; }

        public bool IsSpecial { get; set; }

        public int? NumberOfRegisteredPeople { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (CourseId < 1)
                context.Results.Add(new ValidationResult("CourseId is required"));
        }
    }
}
