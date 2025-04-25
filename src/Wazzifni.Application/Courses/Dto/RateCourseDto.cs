using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wazzifni.Courses.Dto
{
  
    public class RateCourseDto : ICustomValidate
    {
        [Required]
        public double Rate { get; set; }
        [Required]
        public int CourseId { get; set; }

        public virtual void AddValidationErrors(CustomValidationContext context)
        {
            if (Rate > 5)
                context.Results.Add(new ValidationResult("Rate must be between 1 To 5"));



        }
    }
   
}
