using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wazzifni.Attachments.Dto;
using Wazzifni.Domain.Attachments;
using Wazzifni.FileUploadService;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Attachments
{
    /// <summary>
    /// {AttachmentAppService}
    /// </summary>
    public class AttachmentAppService : ApplicationService, IAttachmentAppService
    {
        private readonly IRepository<Attachment, long> _repository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _appBaseUrl;
        private static readonly string AttachmentsFolder = Path.Combine(AppConsts.UploadsFolderName, AppConsts.RecordsFolderName);


        public AttachmentAppService(IRepository<Attachment, long> repository,
            IAttachmentManager attachmentManager, IFileUploadService fileUploadService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _repository = repository;
            _attachmentManager = attachmentManager;
            _fileUploadService = fileUploadService;
            _webHostEnvironment = webHostEnvironment;
            _appBaseUrl = configuration[WazzifniConsts.AppServerRootAddressKey] ?? "/";
        }

        public async Task<ListResultDto<AttachmentDto>> GetAllAsync(PagedAttachmentResultRequestDto input)
        {

            var list = await _repository.GetAllListAsync(x => x.RefId == input.RefId && x.RefType == (AttachmentRefType)input.RefType);
            var listDto = new List<AttachmentDto>();

            foreach (var item in list)
            {
                var itemDto = MapToEntityDto(item);
                itemDto.Url = _attachmentManager.GetUrl(item);

                listDto.Add(itemDto);
            }

            return new ListResultDto<AttachmentDto>(listDto);
        }

        public async Task<AttachmentDto> GetAsync(EntityDto input)
        {
            var entity = await GetEntityByIdAsync(input.Id);

            var entityDto = MapToEntityDto(entity);
            entityDto.Url = _attachmentManager.GetUrl(entity);
            return entityDto;
        }

        public async Task<AttachmentDto> UploadAsync([FromForm] UploadAttachmentInputDto input)
        {
            var attachmentRefType = (AttachmentRefType)input.RefType;
            var fileType = _fileUploadService.GetAndCheckFileType(input.File);
            _attachmentManager.CheckAttachmentRefType(attachmentRefType, fileType);
            await _fileUploadService.CheckFileSizeAsync(input.File);
            var uploadedFileInfo = await _fileUploadService.SaveAttachmentAsync(input.File);
            var attachment = ObjectMapper.Map<Attachment>(input);
            attachment.Type = uploadedFileInfo.Type;
            attachment.Size = uploadedFileInfo.Size;
            attachment.RelativePath = uploadedFileInfo.RelativePath;
            attachment.LowResolutionPhotoRelativePath = uploadedFileInfo.LowResolutionPhotoRelativePath;
            await _repository.InsertAndGetIdAsync(attachment);
            var entityDto = MapToEntityDto(attachment);
            entityDto.Url = _attachmentManager.GetUrl(attachment);
            entityDto.LowResolutionPhotosUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment);
            return entityDto;
        }

        [RemoteService(IsEnabled = false)]
        public async Task DeleteAsync(EntityDto input)
        {
            var entity = await GetEntityByIdAsync(input.Id);
            await _repository.DeleteAsync(entity);
            _fileUploadService.DeleteAttachment(entity.RelativePath);
        }

        private async Task<Attachment> GetEntityByIdAsync(int id)
        {
            var attachment = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (attachment == null)
                throw new EntityNotFoundException(L("AttachmentIsNotFound"));

            return attachment;
        }

        private AttachmentDto MapToEntityDto(Attachment entity)
        {
            return ObjectMapper.Map<AttachmentDto>(entity);
        }

        public async Task<List<AttachmentDto>> UploadMultiAttachmentAsync([FromForm] UploadMultiAttachmentInputDto input)
        {
            var attachmentDtos = new List<AttachmentDto>();
            foreach (var File in input.Files)
            {
                var attachmentRefType = (AttachmentRefType)input.RefType;
                var fileType = _fileUploadService.GetAndCheckFileType(File);
                _attachmentManager.CheckAttachmentRefType(attachmentRefType, fileType);
                await _fileUploadService.CheckFileSizeAsync(File);
                var uploadedFileInfo = await _fileUploadService.SaveAttachmentAsync(File);
                var attachment = ObjectMapper.Map<Attachment>(input);
                attachment.Type = uploadedFileInfo.Type;
                attachment.RelativePath = uploadedFileInfo.RelativePath;
                attachment.LowResolutionPhotoRelativePath = uploadedFileInfo.LowResolutionPhotoRelativePath;
                await _repository.InsertAndGetIdAsync(attachment);
                var entityDto = MapToEntityDto(attachment);
                entityDto.Url = _attachmentManager.GetUrl(attachment);
                entityDto.LowResolutionPhotosUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment);
                attachmentDtos.Add(entityDto);
            }
            return attachmentDtos;
        }





    }
}
