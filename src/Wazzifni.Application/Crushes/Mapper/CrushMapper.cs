using AutoMapper;
using Wazzifni.Crushes.Dto;
using Wazzifni.Domain.Crushes;
using Wazzifni.Domain.Crushes.Dto;

namespace ITLand.StemCells.AnalysisRequests.Mapper
{
    public class CrushMapper : Profile
    {
        public CrushMapper()
        {
            CreateMap<Crush, LiteCrushDto>();
            CreateMap<CreateCrushDto, Crush>();
        }
    }
}
