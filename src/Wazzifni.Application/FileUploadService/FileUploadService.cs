using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Timing;
using Abp.UI;
using Castle.Core.Logging;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wazzifni.Configuration;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;
using static Wazzifni.WazzifniHelper;
namespace Wazzifni.FileUploadService
{

    public class FileUploadService : IFileUploadService
    {
        private static readonly string UploadFolder = Path.Combine(AppConsts.UploadsFolderName);

        private static readonly string AttachmentsFolder = Path.Combine(AppConsts.UploadsFolderName, AppConsts.AttachmentsFolderName);
        private static readonly string VideosFolder = Path.Combine(AppConsts.UploadsFolderName, AppConsts.VideosFolderName);
        private static readonly string LowResolutionPhotosFolder = Path.Combine(AppConsts.UploadsFolderName, AppConsts.LowResolutionPhotosFolderName);
        private static readonly string ImagesFolder = Path.Combine(AppConsts.UploadsFolderName, AppConsts.ImagesFolderName);
        private static readonly string[] extensions = { ".mp4", ".mov", ".avi", ".mpg", ".wmv", ".mkv", ".m4v", ".mpeg", ".ogg" };
        private static readonly string[] Xml = { ".xlsx" };
        private readonly ISettingManager _settingManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILocalizationSource _localizationSource;

        public ILogger Logger { get; set; }

        public FileUploadService(ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _settingManager = settingManager;
            _webHostEnvironment = webHostEnvironment;
            _localizationSource = localizationManager.GetSource(WazzifniConsts.LocalizationSourceName);
            Logger = NullLogger.Instance;
        }

        public async Task<UploadedFileInfo> SaveAttachmentAsync(IFormFile file)
        {
            var fileInfo = new UploadedFileInfo { Type = GetAndCheckFileType(file) };

            var fileName = GenerateUniqueFileName(file);

            if (!Directory.Exists(AttachmentsFolder))
            {
                Directory.CreateDirectory(AttachmentsFolder);
            }

            var pathToSaveAttacment = GetPathToSaveAttachment(fileName, AttachmentsFolder);

            fileInfo.RelativePath = GetAttachmentRelativePath(fileName, AttachmentsFolder);



            var fileSizeInBytes = file.Length;
            fileInfo.Size = FormatFileSize((double)fileSizeInBytes);
            if (fileInfo.Type == AttachmentType.HEIC || fileInfo.Type == AttachmentType.HEIF)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    using (var image = new MagickImage(memoryStream))
                    {
                        image.Format = MagickFormat.Jpeg;
                        fileInfo.Type = AttachmentType.JPG;
                        fileName = Path.ChangeExtension(fileName, ".jpg");
                        pathToSaveAttacment = Path.ChangeExtension(pathToSaveAttacment, ".jpg");
                        fileInfo.RelativePath = GetAttachmentRelativePath(fileName, AttachmentsFolder);
                        await using (var saveStream = new FileStream(pathToSaveAttacment, FileMode.Create))
                        {
                            image.Write(saveStream);
                        }
                    }
                }
            }
            else
            {
                using (var stream = new FileStream(pathToSaveAttacment, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            if (fileInfo.Type == AttachmentType.PNG || fileInfo.Type == AttachmentType.JPG || fileInfo.Type == AttachmentType.JPEG)
            {
                var pathToSaveLowResolutionPhotos = GetPathToSaveAttachment(fileName, LowResolutionPhotosFolder);
                // Load the original image from the saved file
                using (var originalImage = Image.Load(pathToSaveAttacment))
                {
                    var ImageSize = await _settingManager.GetSettingValueAsync<int>(AppSettingNames.ImageSize);
                    // Create and save a version with resolution 200x200
                    originalImage.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(ImageSize),
                        Mode = ResizeMode.Max
                    }));
                    var pathToSaveLowResolutionPhotos200 = GetPathToSaveAttachment(fileName, LowResolutionPhotosFolder);
                    originalImage.Save(pathToSaveLowResolutionPhotos200);
                }
                fileInfo.LowResolutionPhotoRelativePath = GetAttachmentRelativePath(fileName, LowResolutionPhotosFolder);
            }
            Logger.Info($"Base Attachment File was saved to ({pathToSaveAttacment}) successfully.");

            return fileInfo;
        }


        public async Task<UploadedImageInfo> SaveImageAsync(IFormFile file)
        {
            var fileInfo = new UploadedImageInfo { Type = GetAndCheckImageFileType(file) };
            await CheckFileSizeAsync(file);

            var fileName = GenerateUniqueImageFileName(file.FileName);
            var pathToSave = GetPathToSaveImage(fileName);
            using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Logger.Info($"Base Image File was saved to ({pathToSave}) successfully.");

            fileInfo.PathToSave = pathToSave;
            fileInfo.RelativePath = GetImageRelativePath(fileName);
            return fileInfo;
        }


        public UploadedImageInfo GeneratePathToSave(string patientInfo, string folderName)
        {
            var fileName = $"QRImage-{patientInfo}-{Guid.NewGuid()}.png";
            var pathToSave = GetPathToSaveAttachment(fileName, folderName);

            var fileInfo = new UploadedImageInfo
            {
                PathToSave = pathToSave,
                RelativePath = GetAttachmentRelativePath(fileName, folderName),
                Type = ImageType.PNG
            };
            return fileInfo;
        }

        public void DeleteAttachment(string fileRelativePath)
        {
            var pathFile = GetAbsolutePath(fileRelativePath);

            if (!File.Exists(pathFile))
            {
                Logger.Error($"Attachment File ({pathFile}) is not found.");
                return;
            }

            File.Delete(pathFile);

            Logger.Info($"Attachment File ({pathFile}) was deleted successfully.");
        }
        /// <summary>
        /// Check File Size Async
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task CheckFileSizeAsync(IFormFile file)
        {
            var maxFileSize = await _settingManager.GetSettingValueAsync<decimal>(AppSettingNames.FileSize);
            if (file.Length >= maxFileSize * 1024 * 1024)
                throw new UserFriendlyException(Exceptions.FileSizeExceedsMaxFileSize, Exceptions.FileSizeSupported + " " + maxFileSize.ToString());
        }

        public AttachmentType GetAndCheckFileType(IFormFile file)
        {
            var x = file.ContentType;
            var extension = file.FileName.Split('.')
                [file.FileName.Split('.').Length - 1];
            foreach (AttachmentType type in Enum.GetValues(typeof(AttachmentType)))
            {
                if (extension.Equals(AttachmentType.HEIC.ToString().ToLower()))
                    return AttachmentType.HEIC;
                if (extension.Equals(AttachmentType.HEIF.ToString().ToLower()))
                    return AttachmentType.HEIF;
                if (file.ContentType.Contains(type.ToString().ToLower()))
                    return type;
            }
            throw new UserFriendlyException(Exceptions.TheAttachedFileTypeIsNotAcceptable, $"FileName: {file.FileName}");
        }

        private string L(string key)
        {
            return _localizationSource.GetString(key);
        }

        private string L(string key, params object[] args)
        {
            return _localizationSource.GetString(key, args);
        }

        private string GetAbsolutePath(string fileRelativePath)
        {
            var basePath = _webHostEnvironment.WebRootPath;
            return Path.Combine(basePath, fileRelativePath);
        }

        private string GetPathToSaveAttachment(string fileName, string folderName)
        {
            var basePath = _webHostEnvironment.WebRootPath;
            return Path.Combine(basePath, folderName, fileName);
        }

        private string GetAttachmentRelativePath(string fileName, string folderName)
        {
            return Path.Combine(folderName, fileName);
        }

        private string GenerateUniqueFileName(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{Clock.Now.Ticks}{Path.GetExtension(file.FileName)}";
            return fileName;
        }

        private string GetPathToSaveImage(string fileName)
        {
            var basePath = _webHostEnvironment.WebRootPath;
            return Path.Combine(basePath, ImagesFolder, fileName);
        }

        public string GetImageRelativePath(string fileName)
        {
            return Path.Combine(ImagesFolder, fileName);
        }

        private string GenerateUniqueImageFileName(string fileName)
        {
            return $"ItemImage-{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        }

        private ImageType GetAndCheckImageFileType(IFormFile file)
        {
            var extension = file.FileName.Split('.')
                [file.FileName.Split('.').Length - 1];
            foreach (ImageType type in Enum.GetValues(typeof(ImageType)))
            {
                if (extension.Equals(ImageType.HEIC.ToString().ToLower()))
                    return ImageType.HEIC;
                if (extension.Equals(ImageType.HEIF.ToString().ToLower()))
                    return ImageType.HEIF;
                if (file.ContentType.Contains(type.ToString().ToLower()))
                    return type;
            }

            throw new UserFriendlyException(L("UploadedImageFileTypeIsNotAcceptable"), $"FileName: {file.FileName}");
        }

        public UploadedImageInfo GeneratePathToSave(string propertyInfo)
        {

            var fileName = $"QRImage-{propertyInfo}-{Guid.NewGuid()}.png";
            var pathToSave = GetPathToSaveAttachment(fileName);

            var fileInfo = new UploadedImageInfo
            {
                PathToSave = pathToSave,
                RelativePath = GetAttachmentRelativePath(fileName),
                Type = (ImageType)AttachmentType.PNG
            };
            return fileInfo;
        }

        private string GetPathToSaveAttachment(string fileName)
        {
            var basePath = _webHostEnvironment.WebRootPath;
            return Path.Combine(basePath, AttachmentsFolder, fileName);
        }

        private string GetAttachmentRelativePath(string fileName)
        {
            return Path.Combine(AttachmentsFolder, fileName);
        }



        [AbpAllowAnonymous]
        public async Task<UploadVideoInfo> UploadVideo(IFormFile file)
        {
            string fileName = "";
            try
            {
                var extension = '.' + file.FileName.Split('.')
                [file.FileName.Split('.').Length - 1];
                if (extensions.Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                {
                    fileName = DateTime.Now.Ticks.ToString() + extension;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, Wazzifni.AppConsts.UploadsFolderName, Wazzifni.AppConsts.VideosFolderName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var exactPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(exactPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var relativePath = Path.Combine(Wazzifni.AppConsts.UploadsFolderName, Wazzifni.AppConsts.VideosFolderName, fileName);
                    return new UploadVideoInfo
                    {
                        PathToSave = exactPath,
                        Type = extension.ToUpper(),
                        RelativePath = relativePath
                    };
                }
                else
                    throw new UserFriendlyException(L("ErrorVideoExtensionNotAccepted{0}", extension));
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("AnErrorOccured{0}", ex.Message));
            }
        }

        public void DeleteVideo(string fileRelativePath)
        {
            var pathFile = GetAbsolutePath(fileRelativePath);

            if (!File.Exists(pathFile))
            {
                Logger.Error($"Video File ({pathFile}) is not found.");
                return;
            }
            File.Delete(pathFile);
            Logger.Info($"Video File ({pathFile}) was deleted successfully.");
        }








    }
}