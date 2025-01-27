using Abp.Application.Services.Dto;

namespace KeyFinder
{
    public class SwitchActivationInputDto : IEntityDto<int>
    {
        public int Id { get; set; }
    }
}
