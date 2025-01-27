using AutoMapper;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Companies.Dto;


namespace Wazzifni.Companies.Mapper
{
    public class CompanyMapProfile : Profile
    {
        public CompanyMapProfile()
        {
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<CreateCompanyDto, CompanyDto>();
            CreateMap<UpdateCompanyDto, Company>();
            CreateMap<CompanyContactDto, CompanyContact>();
            CreateMap<CompanyContact, CompanyContactDto>();
            CreateMap<CompanyContact, CompanyContactDetailsDto>();
            CreateMap<CompanyDetailsDto, UpdateCompanyDto>();
            CreateMap<CompanyContactDetailsDto, CompanyContactDto>();

        }
    }
}
