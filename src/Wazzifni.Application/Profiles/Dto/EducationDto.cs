using System;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class EducationDto
    {
        public EducationLevel Level { get; set; }
        public string InstitutionName { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyStudying { get; set; }
        public string Description { get; set; }
    }
}
