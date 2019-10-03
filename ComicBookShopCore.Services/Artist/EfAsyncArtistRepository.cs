using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.Artist
{
    public class EfAsyncArtistRepository : IAsyncArtistRepository
    {
        private readonly ShopDbEntities _context;

        public EfAsyncArtistRepository(ShopDbEntities context)
        {
            _context = context;
        }

        public Task<IQueryable<Data.Artist>> GetAllAsync()
        {
            return Task.FromResult( _context.Set<Data.Artist>().AsQueryable());
        }

        public async Task<IEnumerable<Data.Artist>> GetWhereAsync(Expression<Func<Data.Artist, bool>> predicate)
        {
            return await _context.Set<Data.Artist>().Where(predicate).ToListAsync().ConfigureAwait(true);
        }

        public async Task<Data.Artist> GetByIdAsync(int id)
        {
            return await _context.Set<Data.Artist>().FindAsync(id).ConfigureAwait(true);
        }

        public Task AddAsync(Data.Artist artist)
        {
            _context.Set<Data.Artist>().Add(artist);
	    return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Data.Artist artist)
        {
            _context.Set<Data.Artist>().Update(artist);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Data.Artist artist)
        {
            _context.Set<Data.Artist>().Remove(artist);
            return _context.SaveChangesAsync();
        }
    }
}