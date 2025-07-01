using APTXHub.Infrastructure.Dtos;
using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace APTXHub.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllPostsAsync(int loggedInUserId)
        {
            var allPosts = await _context.Posts
                .Where(n => (!n.IsPrivate || n.UserId == loggedInUserId) // Không phải private hoặc là private nhưng thuộc về user hiện tại.
                    && n.Reports.Count < 5
                    && n.IsDeleted == false)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Favorites)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Reports)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            return allPosts;
        }

        public async Task<List<Post>> GetAllFavoritePostsAsync(int loggedInUserId)
        {
            var allFavoritedPosts = await _context.Favorites
                .Include(f => f.Post.User)
                .Include(f => f.Post.Comments)
                    .ThenInclude(c => c.User)
                .Include(f => f.Post.Likes)
                .Include(f => f.Post.Favorites)
                .Where(n => n.UserId == loggedInUserId &&
                    !n.Post.IsDeleted &&
                    n.Post.Reports.Count < 5)
                .OrderByDescending(f => f.DateCreated)
                .Include(n => n.Post)
                .Select(n => n.Post)
                .ToListAsync();

            return allFavoritedPosts;
        }


        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            var post = await _context.Posts
               .Include(n => n.User)
               .Include(n => n.Likes)
               .Include(n => n.Favorites)
               .Include(n => n.Comments).ThenInclude(n => n.User)
               .FirstOrDefaultAsync(n => n.Id == postId);

            return post; // POST MAYBE NULL HERE
        }
        public async Task AddPostCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post?> RemovePostSoftAsync(int postId, int loggedInUserId)
        {
            var post = await _context.Posts
                .Include(p => p.PostHashtags)
                .ThenInclude(ph => ph.Hashtag)
                .FirstOrDefaultAsync(p => p.Id == postId && p.UserId == loggedInUserId);

            if (post == null) return null;

            post.IsDeleted = true;
            post.DeletedAt = DateTime.UtcNow;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task RemovePostCommentAsync(int commentId)
        {
            var commentDb = _context.Comments.FirstOrDefault(n => n.Id == commentId);

            if (commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReportPostAsync(int postId, int userId, string reason)
        {
            // Check if the user has already reported this post
            var existingReport = await _context.Reports
                .FirstOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
            if (existingReport == null)
            {
                // Create a new report
                var newReport = new Report
                {
                    PostId = postId,
                    UserId = userId,
                    Reason = reason,
                    DateCreated = DateTime.UtcNow
                };
                await _context.Reports.AddAsync(newReport);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TogglePostVisibilityAsync(int postId, int userId)
        {
            //get post by id and loggedin user idAdd commentMore actions
            var post = await _context.Posts
                .FirstOrDefaultAsync(l => l.Id == postId && l.UserId == userId);

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }

        // NOTICE
        public async Task<ToggleLikeResult> TogglePostLikeAsync(int postId, int userId)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            bool liked;

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                liked = false;
            }
            else
            {
                var newLike = new Like
                {
                    PostId = postId,
                    UserId = userId
                };
                await _context.Likes.AddAsync(newLike);
                liked = true;
            }

            await _context.SaveChangesAsync();

            int totalLikes = await _context.Likes
                .CountAsync(l => l.PostId == postId);

            return new ToggleLikeResult
            {
                Liked = liked,
                TotalLikes = totalLikes
            };
        }

        // NOTICE
        public async Task<ToggleFavoriteResult> TogglePostFavoriteAsync(int postId, int userId)
        {
            var existingFavorited = await _context.Favorites
                .AsNoTracking()
                 .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            bool favorited;

            if (existingFavorited != null)
            {
                _context.Favorites.Remove(existingFavorited);
                favorited = false;
            }
            else
            {
                var newFavorite = new Favorite
                {
                    PostId = postId,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };
                await _context.Favorites.AddAsync(newFavorite);
                favorited = true;
            }

            await _context.SaveChangesAsync();

            var totalFavorited = await _context.Favorites
               .CountAsync(l => l.PostId == postId);

            return new ToggleFavoriteResult
            {
                Favorited = favorited,
                TotalFavorites = totalFavorited
            };

        }

    }
}
