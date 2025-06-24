using APTXHub.Data.Helpers;
using APTXHub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly AppDbContext _context;

        public HashtagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessHashtagsForNewPostAsync(string content, int newPostId)
        {
            //Find and save hashtags
            var postHashtags = HashtagHelper.GetHashtags(content);
            // Tạo liên kết PostHashtag
            foreach (var hashTag in postHashtags)
            {
                var hashtagDb = await _context.Hashtags
                    .FirstOrDefaultAsync(n => n.Name == hashTag);

                if (hashtagDb != null)
                {
                    hashtagDb.Count += 1;
                    hashtagDb.DateUpdated = DateTime.UtcNow;
                    _context.Hashtags.Update(hashtagDb);
                }
                else
                {
                    hashtagDb = new Hashtag()
                    {
                        Name = hashTag,
                        Count = 1,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };
                    await _context.Hashtags.AddAsync(hashtagDb);
                    await _context.SaveChangesAsync();
                }

                // Tạo PostHashtag
                var postHashtag = new PostHashtag
                {
                    PostId = newPostId,
                    HashtagId = hashtagDb.Id
                };
                await _context.Set<PostHashtag>().AddAsync(postHashtag);
            }

            await _context.SaveChangesAsync();
        }

       
        public async Task ProcessHashtagsForRemovedPostAsync(Post post)
        {
            foreach (var ph in post.PostHashtags)
            {
                ph.Hashtag.Count--;

                if (ph.Hashtag.Count <= 0)
                {
                    _context.Hashtags.Remove(ph.Hashtag);
                }

                _context.PostHashtags.Remove(ph);
            }

            await _context.SaveChangesAsync();
        }
    }
}
