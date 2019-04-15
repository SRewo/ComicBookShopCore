using System.Linq;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Data.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class
    {

        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        public SqlRepository(DbContext dataContext)
        {

            _dbSet = dataContext.Set<T>();
            _dbContext = dataContext;
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {

            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {

            return _dbSet;

        }

        public T GetById(int id)
        {

            return _dbSet.Find(id);

        }


        public void Update(T entity)
        {
            
           _dbSet.Update(entity);
           _dbContext.SaveChanges();
        }

    }
}
