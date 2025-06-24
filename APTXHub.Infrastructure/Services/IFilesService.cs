using APTXHub.Infrastructure.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IFilesService
    {
        Task<string?> UploadMediaAsync(IFormFile file, MediaFileType mediaType);
    }
}
