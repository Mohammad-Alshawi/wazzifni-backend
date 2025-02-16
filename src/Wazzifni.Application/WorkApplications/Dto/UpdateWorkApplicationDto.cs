using Abp.Application.Services.Dto;

namespace Wazzifni.WorkApplications.Dto
{
    public class UpdateWorkApplicationDto : CreateWorkApplicationDto, IEntityDto<long>
    {
        public long Id { get; set; }
    }
}
