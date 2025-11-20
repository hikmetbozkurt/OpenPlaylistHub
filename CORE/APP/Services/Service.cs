using CORE.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace CORE.APP.Services
{
    public abstract class Service<TEntity> : ServiceBase, IDisposable where TEntity : Entity, new()
    {
        private readonly DbContext _dbContext;

        protected DbContext DbContext => _dbContext;

        protected Service(DbContext db)
        {
            _dbContext = db;
        }

        protected virtual IQueryable<TEntity> Query(bool isNoTracking = true)
        {
            return isNoTracking
                ? _dbContext.Set<TEntity>().AsNoTracking()
                : _dbContext.Set<TEntity>();
        }

        protected virtual int SaveChanges() => _dbContext.SaveChanges();

        protected void Create(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString();
            _dbContext.Set<TEntity>().Add(entity);
            if (save)
            {
                SaveChanges();
            }
        }

        protected void Update(TEntity entity, bool save = true)
        {
            _dbContext.Set<TEntity>().Update(entity);
            if (save)
            {
                SaveChanges();
            }
        }

        protected void Delete(TEntity entity, bool save = true)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (save)
            {
                SaveChanges();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
