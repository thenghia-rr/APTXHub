﻿using APTXHub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ApproveReportAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetReportedPostsAsync()
        {
            var posts = await _context.Posts
                 .Where(n => n.NrOfReports == 1 && !n.IsDeleted) // > 5
                 .Include(n => n.User)
                 .Include(n => n.Reports)
                 .ToListAsync();

            return posts;
        }

        public async Task RejectReportAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                postDb.NrOfReports = 0;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }

            var postReports = await _context.Reports.Where(n => n.PostId == postId).ToListAsync();
            if (postReports.Any())
            {
                _context.Reports.RemoveRange(postReports);
                await _context.SaveChangesAsync();
            }
        }
    }
}
