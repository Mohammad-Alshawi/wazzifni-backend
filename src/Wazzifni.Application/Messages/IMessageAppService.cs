using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Messages.Dto;
using Wazzifni.CrudAppServiceBase;
using System;

namespace Wazzifni.Messages
{
    public interface IMessageAppService : IWazzifniAsyncCrudAppService<MessageDetailsDto, Guid, MessageLiteDto, PagedMessageResultRequestDto,
        CreateMessageDto, UpdateMessageDto>
    {

    }
}
