using AutoMapper;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Messages.Dto;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.Messages;

namespace Wazzifni.Messages.Mapper
{
    public class MessageMapProfile : Profile
    {
        public MessageMapProfile()
        {
            CreateMap<CreateMessageDto, Message>();
            CreateMap<UpdateMessageDto, Message>();
            CreateMap<MessageLiteDto, Message>();
            CreateMap<Message, MessageLiteDto>();
            CreateMap<Message, MessageDetailsDto>();
        }
    }
}
