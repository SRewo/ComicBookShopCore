using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.Publisher
{
    public class EfAsyncPublisherRepository : IAsyncPublisherRepository
    {
        private readonly ShopDbEntities _context;

        public EfAsyncPublisherRepository(ShopDbEntities context)
        {
            _context = context;
        }

        public  Task<IQueryable<Data.Publisher>> GetAsync()
        {
            return Task.FromResult(_context.Set<Data.Publisher>().AsQueryable());
        }

        public Task<Data.Publisher> GetByIdAsync(int id)
        {
            return _context.Set<Data.Publisher>().Include(x => x.SeriesList).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task AddAsync(Data.Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Data.Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Data.Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
            return _context.SaveChangesAsync();
        }
    }
}