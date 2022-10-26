using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region constructor
        private SampleDbContext context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(SampleDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }
        #endregion
        public async Task AddEntity(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = entity.CreatedAt;
            await dbSet.AddAsync(entity);
        }

        public IQueryable<TEntity> GetEntitiesQuery()
        {
            return dbSet.AsQueryable();
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
            return await dbSet.SingleOrDefaultAsync(e => e.Id == entityId);
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.IsDelete = true;
            UpdateEntity(entity);
        }

        public async Task RemoveEntity(long entityId)
        {
            var entity = await GetEntityById(entityId);
            RemoveEntity(entity);
        }

        public async Task SaveChanges()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public void UpdateEntity(TEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            dbSet.Update(entity);
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}
