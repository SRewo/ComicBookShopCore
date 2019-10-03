using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.ComicBook
{
    public class EfComicBookRepositoryAsync : IComicBookRepositoryAsync
    {
        private readonly ShopDbEntities _context;

        public EfComicBookRepositoryAsync(ShopDbEntities context)
        {
            _context = context;
        }


        public Task<IQueryable<Data.ComicBook>> GetListAsync()
        {
            return Task.FromResult(_context.Set<Data.ComicBook>().Include(x => x.Series).ThenInclude(x => x.Publisher)
                .AsQueryable());
        }

        public Task<Data.ComicBook> GetByIdAsync(int id)
        {
            return _context.Set<Data.ComicBook>().Include(x => x.Series).ThenInclude(x => x.Publisher)
                .Include(x => x.ComicBookArtists).ThenInclude(x => x.Artist).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task AddAsync(Data.ComicBook comic)
        {
            _context.Set<Data.ComicBook>().Add(comic);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Data.ComicBook comic)
        {
            _context.Update(comic);
	    return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Data.ComicBook comic)
        {
            _context.RemoveRange(comic.ComicBookArtists);
            _context.Remove(comic);
	    return _context.SaveChangesAsync();
        }
    }
}
