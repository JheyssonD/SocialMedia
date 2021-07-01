using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialMediaContext Context;

        public UserRepository(SocialMediaContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            List<User> users = await Context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(int id)
        {
            User user = await Context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task InsertUser(User post)
        {
            Context.Users.Add(post);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUser(User modifiedUser)
        {
            User currentPost = await GetUser(modifiedUser.UserId);
            currentPost.Comments = modifiedUser.Comments;
            currentPost.FirstName = modifiedUser.FirstName;
            currentPost.LastName = modifiedUser.LastName;
            currentPost.Email = modifiedUser.Email;
            currentPost.DateOfBirth = modifiedUser.DateOfBirth;
            currentPost.Phone = modifiedUser.Phone;
            currentPost.LastName = modifiedUser.LastName;
            currentPost.FirstName = modifiedUser.FirstName;
            currentPost.IsActive = modifiedUser.IsActive;
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            User currentUser = await GetUser(id);
            if (currentUser == null)
            {
                return false;
            }
            Context.Users.Remove(currentUser);
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
