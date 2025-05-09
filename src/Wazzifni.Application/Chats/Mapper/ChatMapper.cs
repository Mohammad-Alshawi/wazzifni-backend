using AutoMapper;
using Wazzifni.Chats.Dto;
using Wazzifni.Domain.Messages;


namespace ITLand.Wazzifni.AnalysisRequests.Mapper
{
    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            CreateMap<Chat, LiteChatDto>();
        }
    }
}
