using AutoMapper;
using Wazzifni.Domain.Teachers;
using Wazzifni.Teachers.Dto;

namespace ITLand.StemCells.AnalysisRequests.Mapper
{
    public class TeacherMapper : Profile
    {
        public TeacherMapper()
        {
            CreateMap<Teacher, LiteTeacherDto>();
            CreateMap<CreateTeacherDto, Teacher>();
            CreateMap<UpdateTeacherDto, Teacher>();

            CreateMap<Teacher, TeacherDetailsDto>();

            
        }
    }
}
