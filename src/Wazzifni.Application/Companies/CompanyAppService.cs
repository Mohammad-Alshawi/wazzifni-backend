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
using Wazzifni.Authorization;
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
        private readonly ICompanyManager _companyManager;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IMapper _mapper;

        public CompanyAppService(
            UserManager userManager,
            IRepository<Company> repository,
            ICompanyManager companyManager,
            IAttachmentManager attachmentManager,
            IMapper mapper



        ) : base(repository)
        {
            _userManager = userManager;
            _companyManager = companyManager;
            _attachmentManager = attachmentManager;
            _mapper = mapper;

        }

        [HttpPost, AbpAuthorize(PermissionNames.Companies_Create)]
        public override async Task<CompanyDetailsDto> CreateAsync(CreateCompanyDto input)
        {

            var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

            if (userLogin.Type != UserType.CompanyUser)
                throw new UserFriendlyException(Exceptions.YouCannotDoThisAction);

            if (await Repository.GetAll().AnyAsync(x => x.UserId == userLogin.Id))
                throw new UserFriendlyException(Exceptions.ObjectIsAlreadyExist, "Company User Already Has Company");

            var company = _mapper.Map<Company>(input);
            company.User = userLogin;
            company.Status = CompanyStatus.Checking;

            var companyId = await Repository.InsertAndGetIdAsync(company);
            userLogin.CompanyId = companyId;
            await _userManager.UpdateAsync(userLogin);

            UnitOfWorkManager.Current.SaveChanges();

            if (input.CompanyProfilePhotoId != 0)
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                       input.CompanyProfilePhotoId, AttachmentRefType.CompanyLogo, company.Id);

            if (input.Attachments is not null && input.Attachments.Count > 0)
            {
                foreach (var attachmentId in input.Attachments)
                {
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                        attachmentId, AttachmentRefType.CompanyImage, company.Id);
                }
            }
            return _mapper.Map<CompanyDetailsDto>(company);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task DeleteAsync(EntityDto<int> input)
        {
            return base.DeleteAsync(input);
        }

        [HttpPut, AbpAuthorize(PermissionNames.Companies_Update)]

        public override async Task<CompanyDetailsDto> UpdateAsync(UpdateCompanyDto input)
        {
            var company = await _companyManager.GetSuperLiteEntityByIdAsync(input.Id);
            var oldStaus = company.Status;
            var oldActivs = company.IsActive;

            company.Translations.Clear();

            company = _mapper.Map(input, company);

            company.Status = oldStaus;
            company.IsActive = oldActivs;
            await Repository.UpdateAsync(company);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var oldimagesAttachments = await _attachmentManager.GetByRefAsync(company.Id, AttachmentRefType.CompanyImage);
            var oldimagesAttachmentsIds = oldimagesAttachments.Select(x => x.Id).ToList();
            var imagesattachmentsToDelete = oldimagesAttachments.Where(x => !input.Attachments.Contains((x.Id)));
            var imagesattachmentIdsToAdd = input.Attachments.Except(oldimagesAttachments.Select(x => x.Id).ToList());
            foreach (var attachment in imagesattachmentsToDelete)
            {
                await _attachmentManager.DeleteRefIdAsync(attachment);
            }
            foreach (var attachmentId in imagesattachmentIdsToAdd)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                    attachmentId, AttachmentRefType.CompanyImage, company.Id);
            }
            var oldAttachment = await _attachmentManager.GetElementByRefAsync(company.Id, AttachmentRefType.CompanyLogo);

            if (input.CompanyProfilePhotoId == 0 && oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            else if (input.CompanyProfilePhotoId != 0 && oldAttachment is not null)
            {
                if (oldAttachment.Id != input.CompanyProfilePhotoId)
                {
                    await _attachmentManager.DeleteRefIdAsync(oldAttachment);
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                     input.CompanyProfilePhotoId, AttachmentRefType.CompanyLogo, company.Id);
                }
            }
            else if (input.CompanyProfilePhotoId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.CompanyProfilePhotoId, AttachmentRefType.CompanyLogo, company.Id);
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return _mapper.Map<CompanyDetailsDto>(company);
        }

        public override async Task<CompanyDetailsDto> GetAsync(EntityDto<int> input)
        {
            var company = await _companyManager.GetFullEntityByIdAsync(input.Id);

            var result = _mapper.Map<CompanyDetailsDto>(company);

            var profile = await _attachmentManager.GetElementByRefAsync(input.Id, AttachmentRefType.CompanyLogo);
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
            if (profile is not null)
            {

                result.Profile = new LiteAttachmentDto
                {
                    Id = profile.Id,
                    Url = _attachmentManager.GetUrl(profile),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(profile),
                };


            }
            return result;
        }


        [HttpPost, AbpAuthorize(PermissionNames.Companies_Approve)]
        public async Task<bool> ApproveAsync(EntityDto<int> input)
        {
            var company = await _companyManager.GetLiteCompanyByIdAsync(input.Id);
            company.Status = CompanyStatus.Approved;
            company.ApprovedDate = DateTime.Now;
            await Repository.UpdateAsync(company);
            return true;
        }


        [HttpPost, AbpAuthorize(PermissionNames.Companies_Reject)]
        public async Task<bool> RejectAsync(CompanyRejectInputDto input)
        {
            var company = await _companyManager.GetLiteCompanyByIdAsync(input.CompanyId);
            company.Status = CompanyStatus.Rejected;
            company.ReasonRefuse = input.ReasonRefuse;
            await Repository.UpdateAsync(company);
            return true;
        }





        public override async Task<PagedResultDto<LiteCompanyDto>> GetAllAsync(PagedCompanyResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.CompanyLogo);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Id, out var itemAttachments))
                {
                    item.Profile = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }
            return result;
        }

        protected override IQueryable<Company> CreateFilteredQuery(PagedCompanyResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.User);
            data = data.Include(x => x.City).ThenInclude(x => x.Translations);
            data = data.Include(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Translations);

            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.JobType.ToString().Contains(input.Keyword));

            if (input.CityId.HasValue)
                data = data.Where(x => x.CityId == input.CityId.Value);

            return data;
        }
        protected override IQueryable<Company> ApplySorting(IQueryable<Company> query, PagedCompanyResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
