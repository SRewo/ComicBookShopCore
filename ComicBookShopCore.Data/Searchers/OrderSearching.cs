using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Data.Searchers
{
    public static class OrderSearching
    {
        public static Task<List<Order>> RoleSearchAsync(this List<Order> list, IRoleSearcher searcher)
        {
            return Task.FromResult(list.Where( x => Task.Run(() => searcher.IsInRolesAsync(x.Employee)).Result).ToList());
        }

    }
}