using APTXHub.Infrastructure.Helpers.Enums;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class FilesSevice : IFilesService
    {
        private readonly Cloudinary _cloudinary;

        public FilesSevice(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadMediaAsync(IFormFile file, MediaFileType mediaType)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLower();
            var contentType = file.ContentType.ToLower();

            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedVideoExtensions = new[] { ".mp4", ".webm", ".ogg" };

            bool isImage = contentType.StartsWith("image/") && allowedImageExtensions.Contains(ext);
            bool isVideo = contentType.StartsWith("video/") && allowedVideoExtensions.Contains(ext);

            if (!isImage && !isVideo)
                return null;

            await using var stream = file.OpenReadStream();

            if (isImage)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = GetFolder(mediaType)
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl?.ToString();
            }
            else if (isVideo)
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = GetFolder(mediaType),
                    //ResourceType = ResourceType.Video
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl?.ToString();
            }

            return null;
        }

        private string GetFolder(MediaFileType type)
        {
            return type switch
            {
                MediaFileType.StoryMedia => "APTXHub/stories",
                MediaFileType.PostImage => "APTXHub/posts",
                MediaFileType.ProfilePicture => "APTXHub/profiles",
                MediaFileType.CoverImage => "APTXHub/covers",
                MediaFileType.ReelVideo => "APTXHub/reels",
                _ => "APTXHub/others"
            };
        }

        // SAVE IN MEMORY

        //public async Task<string?> UploadMediaAsync(IFormFile file, MediaFileType mediaType)
        //{
        //    if (file == null || file.Length == 0) return null;

        //    var ext = Path.GetExtension(file.FileName).ToLower();
        //    var contentType = file.ContentType.ToLower();

        //    // Danh sách cho phép
        //    var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        //    var allowedVideoExtensions = new[] { ".mp4", ".webm", ".ogg" };

        //    bool isImage = contentType.StartsWith("image/") && allowedImageExtensions.Contains(ext);
        //    bool isVideo = contentType.StartsWith("video/") && allowedVideoExtensions.Contains(ext);

        //    if (!isImage && !isVideo)
        //        return null;

        //    // Xác định folder lưu
        //    string folder = mediaType switch
        //    {
        //        MediaFileType.StoryMedia => isVideo ? "videos/stories" : "images/stories",
        //        MediaFileType.PostImage => "images/posts",
        //        MediaFileType.ProfilePicture => "images/profilePictures",
        //        MediaFileType.CoverImage => "images/covers",
        //        MediaFileType.ReelVideo => "videos/reels",
        //        _ => throw new ArgumentOutOfRangeException(nameof(mediaType), "Invalid media type")
        //    };

        //    string fileName = Guid.NewGuid() + ext;
        //    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
        //    Directory.CreateDirectory(rootPath);

        //    string fullPath = Path.Combine(rootPath, fileName);

        //    using var stream = new FileStream(fullPath, FileMode.Create);
        //    await file.CopyToAsync(stream);

        //    return $"/{folder}/{fileName}";
        //}
    }

}
