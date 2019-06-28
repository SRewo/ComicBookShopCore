using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Data.Filters
{
    public static class OrderSearching
    {
        public static List<Order> RoleSearchAsync(this List<Order> list, IRoleFilter filter)
        {
            return list.Where( x => Task.Run(() => filter.IsInRolesAsync(x.Employee)).Result).ToList();
        }

    }
}