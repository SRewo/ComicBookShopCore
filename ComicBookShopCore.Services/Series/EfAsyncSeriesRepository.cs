using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.Series
{
    public class EfAsyncSeriesRepository : IAsyncSeriesRepository
    {
        private readonly ShopDbEntities _context;

        public EfAsyncSeriesRepository(ShopDbEntities context)
        {
            _context = context;
        }

        public Task<IQueryable<Data.Series>> GetAsync()
        {
            return Task.FromResult(_context.Set<Data.Series>().Include(x => x.Publisher).AsQueryable());
        }

        public Task<Data.Series> GetByIdAsync(int id)
        {
            return _context.Set<Data.Series>().Include(x => x.Publisher).Include(x => x.ComicBooks)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task AddAsync(Data.Series series)
        {
            _context.Set<Data.Series>().Add(series);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Data.Series series)
        {
            _context.Set<Data.Series>().Update(series);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Data.Series series)
        {
            _context.Set<Data.Series>().Remove(series);
            return _context.SaveChangesAsync();
        }
    }
}