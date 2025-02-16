using AutoMapper;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.WorkApplications.Dto;

namespace Wazzifni.WorkApplications.Mapper
{

    public class WorkApplicationMapperWorkApplication : Profile
    {
        public WorkApplicationMapperWorkApplication()
        {

            CreateMap<CreateWorkApplicationDto, WorkApplication>();
            CreateMap<CreateWorkApplicationDto, WorkApplicationDetailsDto>();
            CreateMap<WorkApplicationDetailsDto, WorkApplication>();
            CreateMap<WorkApplication, WorkApplicationDetailsDto>();
            CreateMap<UpdateWorkApplicationDto, WorkApplication>();
            CreateMap<WorkApplication, WorkApplicationLiteDto>();




        }
    }
}
