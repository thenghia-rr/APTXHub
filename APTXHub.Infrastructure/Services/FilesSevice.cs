using APTXHub.Infrastructure.Helpers.Enums;
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

        public async Task<string?> UploadMediaAsync(IFormFile file, MediaFileType mediaType)
        {
            if (file == null || file.Length == 0) return null;

            var ext = Path.GetExtension(file.FileName).ToLower();
            var contentType = file.ContentType.ToLower();

            // Danh sách cho phép
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedVideoExtensions = new[] { ".mp4", ".webm", ".ogg" };

            bool isImage = contentType.StartsWith("image/") && allowedImageExtensions.Contains(ext);
            bool isVideo = contentType.StartsWith("video/") && allowedVideoExtensions.Contains(ext);

            if (!isImage && !isVideo)
                return null;

            // Xác định folder lưu
            string folder = mediaType switch
            {
                MediaFileType.StoryMedia => isVideo ? "videos/stories" : "images/stories",
                MediaFileType.PostImage => "images/posts",
                MediaFileType.ProfilePicture => "images/profilePictures",
                MediaFileType.CoverImage => "images/covers",
                MediaFileType.ReelVideo => "videos/reels",
                _ => throw new ArgumentOutOfRangeException(nameof(mediaType), "Invalid media type")
            };

            string fileName = Guid.NewGuid() + ext;
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            Directory.CreateDirectory(rootPath);

            string fullPath = Path.Combine(rootPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }
    }

}
