using AutoMapper;
using Wazzifni.Domain.Skills;
using Wazzifni.Skills.Dto;

namespace Wazzifni.Skills.Mapper
{
    public class SkillMapProfile : Profile
    {
        public SkillMapProfile()
        {
            CreateMap<CreateSkillDto, Skill>();
            CreateMap<CreateSkillDto, SkillDto>();
            CreateMap<SkillDto, Skill>();
            CreateMap<UpdateSkillDto, Skill>();

        }
    }
}
