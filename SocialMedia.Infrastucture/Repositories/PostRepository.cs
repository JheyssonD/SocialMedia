using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialMediaContext Context;

        public PostRepository(SocialMediaContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var post = await  Context.Posts.ToListAsync();
            return post;
        }
    }
}
