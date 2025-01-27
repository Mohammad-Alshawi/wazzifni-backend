using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Domain.Companies.Dto
{
    public class UpdateCompanyDto : CreateCompanyDto, IEntityDto
    {
        [Required]
        public int Id { get; set; }

    }
}
