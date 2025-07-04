using APTXHub.Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{

    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SearchResultDto> SearchAsync(string query, ClaimsPrincipal user)
        {
            int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int loggedInUserId);

            query = query.Trim().ToLower();

            var posts = await _context.Posts
                .Where(p =>
                    !p.IsDeleted &&
                    p.Reports.Count < 5 &&
                    (p.Content != null && p.Content.ToLower().Contains(query)) &&
                    (p.IsPrivate == false || p.UserId == loggedInUserId)
                )
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Favorites)
                .ToListAsync();

            var users = await _context.Users
                .Where(u =>
                    !u.IsDeleted &&
                    (u.FullName.ToLower().Contains(query) || u.UserName.ToLower().Contains(query))
                )
                .ToListAsync();

            return new SearchResultDto
            {
                Query = query,
                Posts = posts,
                Users = users
            };
        }
    }

}