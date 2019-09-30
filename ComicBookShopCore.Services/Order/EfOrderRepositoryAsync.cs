using System;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.Order
{
    public class EfOrderRepositoryAsync : IOrderRepositoryAsync
    {
        private readonly ShopDbEntities _context;

        public EfOrderRepositoryAsync(ShopDbEntities context)
        {
            _context = context;
        }

        public Task<IQueryable<Data.Order>> GetAllAsync()
        {
            var list = _context.Orders.Include(x => x.OrderItems).Include(x => x.User).AsQueryable();
            return Task.FromResult(list);
        }

        public Task<Data.Order> GetByIdAsync(int id)
        {
            var order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.ComicBook).Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task AddAsync(Data.Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var comic = await _context.ComicBooks.FindAsync(item.ComicBookId);

                if (comic == null)
                    throw new NullReferenceException("Invalid comic book Id");

                if (comic.Quantity < item.Quantity)
                    throw new InvalidOperationException(
                        $"Comic book with id: {comic.Id} does not have enough items to create your order (in stock {comic.Quantity}, your order {item.Quantity})");

                comic.Quantity = comic.Quantity - item.Quantity;
                _context.Update(comic);
            }
            _context.Orders.Add(order);
           await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(Data.Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var comic = await _context.ComicBooks.FindAsync(item.ComicBookId);

                if (comic == null)
                    throw new NullReferenceException("Invalid comic book Id");

                comic.Quantity = comic.Quantity + item.Quantity;
                _context.Update(comic);
            }
            _context.RemoveRange(order.OrderItems);
            _context.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}