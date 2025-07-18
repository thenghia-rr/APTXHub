﻿using APTXHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IUserService
    {
        Task<User> GetUser(int loggedInUserId);
        Task UpdateUserProfilePicture(int loggedInUserId, string profilePictureUrl);
        Task UpdateUserCoverPicture(int loggedInUserId, string coverPictureUrl);
        Task<List<Post>> GetUserPosts(int userId, int loggedInUserId);

        Task<List<User>> GetUserFriends(int userId);
    }
}
