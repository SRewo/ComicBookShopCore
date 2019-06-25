using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ComicBookShopCore.Data.Searchers
{
    public interface IRoleSearcher
    {
        Dictionary<string, bool> Roles { get; }
        Task<bool> IsInRolesAsync(User user);
    }

    public class RoleSearcher : IRoleSearcher
    {
        private UserStore<User> _userStore;
        private readonly ShopDbEntities _context;
        internal RoleSearcher(Dictionary<string, bool> roles, ShopDbEntities context)
        {
            Roles = roles;
            _context = context;
        }

        public Dictionary<string, bool> Roles { get; }
        public async Task<bool> IsInRolesAsync(User user)
        {
            _userStore = new UserStore<User>(_context);
            var tasks = new List<Task<bool>>();
            foreach (var role in Roles)
            {
                tasks.Add(CheckRoleAsync(role, user));
            }

            var result = await Task.WhenAll(tasks).ConfigureAwait(true);

            return result.Any(x => x.Equals(true));
        }

        private Task<bool> CheckRoleAsync(KeyValuePair<string, bool> role, User user)
        {
            return Task.FromResult(role.Value && _userStore.IsInRoleAsync(user, role.Key).Result);
        }
    }
}
