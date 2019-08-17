using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Data.Filters
{
    public static class OrderSearching
    {
        public static List<Order> RoleFilter(this List<Order> list, IRoleFilter filter)
        {
            return list.Where( x => Task.Run(() => filter.IsInRolesAsync(x.User)).Result).ToList();
        }

        public static List<Order> DateFilter(this List<Order> list, DateTime from, DateTime to)
        {
            if (to < from)
                throw new InvalidOperationException("Start date cannot be larger than end date.");

            return list.Where(x => x.Date <= to && x.Date >= from).ToList();
        }

        public static List<Order> NameFilter(this List<Order> list, string name)
        {
            return string.IsNullOrWhiteSpace(name) ? list : list.Where(x => x.User.Name.ToLower().Contains(name.ToLower())).ToList();
        }

    }
}