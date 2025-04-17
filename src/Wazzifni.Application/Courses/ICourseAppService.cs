using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wazzifni.Courses.Dto;
using Wazzifni.CrudAppServiceBase;

namespace Wazzifni.Courses
{
    public interface ICourseAppService : IWazzifniAsyncCrudAppService<CourseDetailsDto, int, CourseLiteDto, PagedCourseResultRequestDto,
        CreateCourseDto, UpdateCourseDto>
    {
    }
}
