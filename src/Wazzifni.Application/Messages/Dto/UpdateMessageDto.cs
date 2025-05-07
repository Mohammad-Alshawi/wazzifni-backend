using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Wazzifni.CourseCategories.Dto;

namespace Wazzifni.Messages.Dto
{
    public class UpdateMessageDto : CreateMessageDto ,IEntityDto<Guid>
    {
        [Required]
        public Guid Id { get; set; }

    }
}
