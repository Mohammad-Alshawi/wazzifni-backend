using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Companies.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Companies.Dto;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Companies
{
    public class CompanyAppService :
         WazzifniAsyncCrudAppService<Company, CompanyDetailsDto, int, LiteCompanyDto, PagedCompanyResultRequestDto, CreateCompanyDto, UpdateCompanyDto>,
         ICompanyAppService
    {
        private readonly UserManager _userManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IMapper _mapper;

        public CompanyAppService(
            UserManager userManager,
            IRepository<Company> repository,
            IAttachmentManager attachmentManager,
            IMapper mapper



        ) : base(repository)
        {
            _userManager = userManager;
            _attachmentManager = attachmentManager;
            _mapper = mapper;

        }

        [HttpPost, AbpAllowAnonymous]
        public override async Task<CompanyDetailsDto> CreateAsync(CreateCompanyDto input)
        {
            var Company = _mapper.Map<Company>(input);
            Company.CreationTime = DateTime.UtcNow;

            if (AbpSession.UserId.HasValue)
            {
                var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                if (userLogin.Type == UserType.Admin)
                {
                    // User user = await _userRegistrationManager.RegisterAsyncForUserCompanyByAdmin(input.userDto.PhoneNumber, input.userDto.DialCode, UserType.CompanyUser);
                    // Company.User = user;
                    // await AddDefaultBundleToCompany(Company);
                    // Company.Status = CompanyStatus.Approved;
                }
                else if (userLogin.Type == UserType.CompanyUser)
                {
                    if (await Repository.GetAll().AnyAsync(x => x.UserId == userLogin.Id))
                        throw new UserFriendlyException(Exceptions.ObjectIsAlreadyExist, Tokens.User + " User Already Has Company");
                    Company.User = userLogin;
                    Company.Status = CompanyStatus.Checking;
                }

                else
                    throw new UserFriendlyException(Exceptions.YouCannotDoThisAction);
            }
            else
            {
                throw new UserFriendlyException(Exceptions.YouCannotDoThisAction);
            }

            // var Contacts = _mapper.Map<List<CreateCompanyContactDto>, List<CompanyContact>>(input.CompanyContactDtos);
            // Company.CompanyContact = Contacts;

            await Repository.InsertAndGetIdAsync(Company);
            UnitOfWorkManager.Current.SaveChanges();

            if (input.CompanyProfilePhotoId != 0)
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                       input.CompanyProfilePhotoId, AttachmentRefType.CompanyLogo, Company.Id);

            if (input.Attachments is not null && input.Attachments.Count > 0)
            {
                foreach (var attachmentId in input.Attachments)
                {
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                        attachmentId, AttachmentRefType.CompanyImage, Company.Id);
                }
            }
            return _mapper.Map<CompanyDetailsDto>(Company);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task<CompanyDetailsDto> UpdateAsync(UpdateCompanyDto input)
        {
            return base.UpdateAsync(input);
        }

        public override async Task<CompanyDetailsDto> GetAsync(EntityDto<int> input)
        {
            var result = await base.GetAsync(input);
            var attachments = await _attachmentManager.GetByRefAsync(input.Id, AttachmentRefType.CompanyImage);
            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        result.Attachments.Add(new LiteAttachmentDto
                        {
                            Id = attachment.Id,
                            Url = _attachmentManager.GetUrl(attachment),
                            LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                        });
                    }
                }
            }
            return result;
        }
        //
        public override async Task<PagedResultDto<LiteCompanyDto>> GetAllAsync(PagedCompanyResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.CompanyImage);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Id, out var itemAttachments))
                {
                    item.Attachments = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .ToList();
                }
                else
                    item.Attachments = new List<LiteAttachmentDto>();

            }
            return result;
        }

        protected override IQueryable<Company> CreateFilteredQuery(PagedCompanyResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.User);
            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x =>
                                       x.JobType.ToString().Contains(input.Keyword));


            return data;
        }
        protected override IQueryable<Company> ApplySorting(IQueryable<Company> query, PagedCompanyResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
