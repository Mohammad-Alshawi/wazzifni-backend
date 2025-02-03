using System;

namespace Wazzifni.Profiles.Dto
{
    public class WorkExperienceDto
    {
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentJob { get; set; }
        public string Description { get; set; }
    }
}
