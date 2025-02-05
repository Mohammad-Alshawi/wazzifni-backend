using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Companies
{
    public class CompanyManager : DomainService, ICompanyManager
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IMapper _mapper;

        //private readonly IRepository<CompanyContact> _companyContactRepository;
        private readonly IRepository<CompanyTranslation> _companyTranslationsRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly ICityManager _cityManager;

        private readonly UserManager _userManager;
        public CompanyManager(
            IRepository<CompanyTranslation> companyTranslationsRepository,
            IAttachmentManager attachmentManager,
            ICityManager cityManager,
            //IRepository<CompanyContact> companyContactRepository,
            IRepository<Company> companyRepository,
            IMapper mapper,
            UserManager userManager)
        {
            // _companyContactRepository = companyContactRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _companyTranslationsRepository = companyTranslationsRepository;
            _attachmentManager = attachmentManager;
            _cityManager = cityManager;

            _userManager = userManager;
        }
        public async Task<Company> GetSuperLiteEntityByIdAsync(int id)
        {
            return await _companyRepository.GetAsync(id);
        }
        public async Task<Company> GetLiteEntityByIdAsync(int id)
        {
            return await _companyRepository
                .GetAllIncluding(x => x.Translations)
                .Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Company> GetEntityByIdAsync(int id)
        {
            return await _companyRepository.GetAll()
                .AsNoTrackingWithIdentityResolution().
                Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Company> GetLiteCompanyByIdAsync(int id)
        {
            return await _companyRepository
                .GetAllIncluding(x => x.Translations)

                .AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }








        public async Task<List<Company>> GetListOfCompany(List<int> companyIds)
        {
            return await _companyRepository.GetAll().Where(x => companyIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> CheckIfUserHasCompany(long userId)
        {
            return await _companyRepository.GetAll().AsNoTrackingWithIdentityResolution().AnyAsync(x => x.UserId == userId);
        }

        public async Task<bool> CheckIfCompanyExict(int companyId)
        {
            if (!await _companyRepository.GetAll().AnyAsync(x => x.Id == companyId))
            {
                throw new UserFriendlyException(404, Exceptions.ObjectWasNotFound, "Company" + " " + companyId.ToString());

            }
            return true;
        }
        public async Task UpdateAttachmentTypeListAsync(List<long> newAttachmentIds, AttachmentRefType attachmentType, long companyId)
        {
            var existingAttachments = await _attachmentManager.GetByRefAsync(companyId, attachmentType);
            var imagesattachmentsToDelete = existingAttachments.Where(x => !newAttachmentIds.Contains((x.Id)));
            var imagesattachmentIdsToAdd = newAttachmentIds.Except(existingAttachments.Select(x => x.Id).ToList());
            foreach (var existingAttachment in imagesattachmentsToDelete)
            {
                await _attachmentManager.DeleteRefIdAsync(existingAttachment);
            }

            foreach (var newAttachmentId in imagesattachmentIdsToAdd)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                    newAttachmentId, attachmentType, companyId);
            }
        }


        public async Task HardDeleteCompanyTranslation(List<CompanyTranslation> translations)
        {
            try
            {
                foreach (var translation in translations)
                {

                    await _companyTranslationsRepository.HardDeleteAsync(translation);
                }
            }
            catch (Exception ex) { throw; }
        }



        public async Task<int> GetCompnayIdByUserId(long userId)
        {
            return await _companyRepository.GetAll().AsNoTrackingWithIdentityResolution().Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        }



        public async Task<int> GetCompaniesCount()
        {
            return await _companyRepository.GetAll().AsNoTracking().Where(x => x.IsDeleted == false).CountAsync();
        }



        public async Task<long> GetUserIdByCompanyIdAsync(int companyId)
        {
            return await _companyRepository.GetAll()
                            .AsNoTrackingWithIdentityResolution()
                            .Where(x => x.Id == companyId)
                            .Select(x => x.UserId.Value)
                            .FirstOrDefaultAsync();

        }

        public async Task DeleteUpdatedCompanyInstance(int companyId)
        {
            await _companyRepository.GetAll().Where(x => x.Id == companyId).ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true));
        }


        /* public async Task<GeneralRatingDto> GetGeneralRatingDtoForComapny(int companyId)
         {
             //var companyId = _offerManager.GetEntityByIdAsync()
             var result = await _reviewRepository
                 .GetAll().Include(x => x.Offer).ThenInclude(x => x.SelectedCompanies)
                 .AsNoTracking()
                 .Where(x => x.Offer.SelectedCompanies.CompanyId.Value == companyId)
                 .GroupBy(x => 1) // Group all reviews into a single group
                 .Select(group => new GeneralRatingDto
                 {
                     Quality = group.Average(x => x.Quality),
                     OverallRating = group.Average(x => x.OverallRating),
                     ValueOfServiceForMoney = group.Average(x => x.ValueOfServiceForMoney),
                     CustomerService = group.Average(x => x.CustomerService)
                 })
                .FirstOrDefaultAsync();
             if (result is null) return new GeneralRatingDto();
             return result;
         }*/


        /*        public async Task<List<ReviewDetailsDto>> GetReviewsForCompany(int companyId)
                {
                    using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
                    {
                        var reviews = await _reviewRepository
                      .GetAll().Include(x => x.Offer).ThenInclude(x => x.SelectedCompanies).Include(x => x.User)
                      .AsNoTracking()
                      .Where(x => x.Offer.SelectedCompanies.CompanyId.Value == companyId)
                      .Select(x => _mapper.Map<ReviewDetailsDto>(x)).ToListAsync();
                        return reviews;
                    }
                }*/
        public async Task<CompanyStatus> GetCompanyStatusByUserIdAsync(long userId)
        {
            return await _companyRepository.GetAll().Where(x => x.UserId == userId).Select(x => x.Status).FirstOrDefaultAsync();
        }

        public async Task<int> GetCompanyIdByUserId(long userId)
        {
            return await _companyRepository.GetAll().Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        }

    }
}