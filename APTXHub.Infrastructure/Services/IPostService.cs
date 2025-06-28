using APTXHub.Infrastructure.Dtos;
using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int loggedInUserId); 
        Task<List<Post>> GetAllFavoritePostsAsync(int loggedInUserId);
        Task<Post?> GetPostByIdAsync(int postId);

        Task<Post> CreatePostAsync(Post post);
        Task<Post?> RemovePostSoftAsync(int postId, int loggedInUserId);

        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);

        Task<ToggleLikeResult> TogglePostLikeAsync(int postId, int userId);
        Task<ToggleFavoriteResult> TogglePostFavoriteAsync(int postId, int userId);
        Task TogglePostVisibilityAsync(int postId, int userId);
        Task ReportPostAsync(int postId, int userId, string reason);
    }
}
