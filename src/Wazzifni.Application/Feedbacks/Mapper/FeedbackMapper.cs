using AutoMapper;
using Wazzifni.Domain.Feedbacks;
using Wazzifni.Feedbacks.Dto;

namespace ITLand.StemCells.AnalysisRequests.Mapper
{
    public class FeedbackMapper : Profile
    {
        public FeedbackMapper()
        {
            CreateMap<Feedback, LiteFeedbackDto>();
            CreateMap<CreateFeedbackDto, Feedback>();
        }
    }
}
