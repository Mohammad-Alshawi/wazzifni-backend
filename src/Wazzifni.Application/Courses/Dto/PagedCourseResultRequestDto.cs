using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Wazzifni.Courses.Dto
{
    public class PagedCourseResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public int? CityId { get; set; }

        public int? CourseCategoryId { get; set; }

        public int? TeacherId { get; set; }

        public List<int>? TagsId { get; set; }
    }
}
