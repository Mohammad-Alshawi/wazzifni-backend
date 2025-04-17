

using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Wazzifni.Domain.Courses
{
    public class CourseTranslation  : FullAuditedEntity, IEntityTranslation<Course>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Course Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
        
    }

}
