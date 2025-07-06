using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using APTXHub.Infrastructure.Services;
using APTXHub.Infrastructure.Helpers.Enums;

namespace APTXHub.Controllers
{
    [Authorize]
    public class ReelsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFilesService _filesService;

        public ReelsController(AppDbContext context,
            IWebHostEnvironment env,
            IFilesService filesService)
        {
            _context = context;
            _env = env;
            _filesService = filesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reels = await _context.Reels.Include(r => r.User)
                                            .OrderByDescending(r => r.CreatedAt)
                                            .ToListAsync();

            return View(reels);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile videoFile, string? caption)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("Video is required.");

            var videoUrl = await _filesService.UploadMediaAsync(videoFile, MediaFileType.ReelVideo);

            if (videoUrl == null)
                return BadRequest("Unsupported file format.");

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var reel = new Reel
            {
                VideoUrl = videoUrl,
                Caption = caption,
                UserId = currentUserId
            };

            _context.Reels.Add(reel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }   
}
