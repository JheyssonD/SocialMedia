using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastucture.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastucture.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly SocialMediaContext Context;
        private readonly DbSet<T> Entities;

        public BaseRepository(SocialMediaContext context)
        {
            Context = context;
            Entities = Context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<bool> Add(T entity)
        {
            Entities.Add(entity);
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Update(T entity)
        {
            Entities.Update(entity);
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> Delete(int id)
        {
            T currentEntity = await GetById(id);
            Entities.Remove(currentEntity);
            int rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
