﻿using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext Context;
        private readonly IPostRepository _postRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly ISecurityRepository _securityRepository;

        public UnitOfWork(SocialMediaContext context)
        {
            Context = context;
        }

        public IPostRepository PostRepository => _postRepository ?? new PostRepository(Context);
        public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(Context);
        public IRepository<Comment> CommentRepository => _commentRepository ?? new BaseRepository<Comment>(Context);
        public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(Context);

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
