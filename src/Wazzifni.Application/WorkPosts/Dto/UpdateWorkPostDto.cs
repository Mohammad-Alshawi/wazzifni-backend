using Abp.Application.Services.Dto;

namespace Wazzifni.WorkPosts.Dto
{
    public class UpdateWorkPostDto : CreateWorkPostDto, IEntityDto<long>
    {
        public long Id { get; set; }
    }
}
