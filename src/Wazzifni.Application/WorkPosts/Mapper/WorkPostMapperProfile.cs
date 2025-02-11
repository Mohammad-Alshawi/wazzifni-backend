using AutoMapper;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.WorkPosts.Dto;

namespace Wazzifni.WorkPosts.Mapper
{

    public class WorkPostMapperWorkPost : Profile
    {
        public WorkPostMapperWorkPost()
        {

            CreateMap<CreateWorkPostDto, WorkPost>();
            CreateMap<CreateWorkPostDto, WorkPostDetailsDto>();
            CreateMap<WorkPostDetailsDto, WorkPost>();
            CreateMap<WorkPost, WorkPostDetailsDto>();
            CreateMap<UpdateWorkPostDto, WorkPost>();
            CreateMap<WorkPost, WorkPostLiteDto>();




        }
    }
