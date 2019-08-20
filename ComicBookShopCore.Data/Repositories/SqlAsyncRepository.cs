using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Data.Repositories
{
    public class SqlAsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        private DbContext _context;
        private DbSet<T> _dbSet;

        public SqlAsyncRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync().ConfigureAwait(true);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(true);
        }

        public Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}