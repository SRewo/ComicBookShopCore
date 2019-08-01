using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ComicBookShopCore.Data.Filters
{
    public interface IRoleFilter
    {
        Dictionary<string, bool> Roles { get; }
        Task<bool> IsInRolesAsync(User user);
    }

    public class RoleFilter : IRoleFilter
    {
        private static readonly Dictionary<string, IList<User>> _usersInRole = new Dictionary<string, IList<User>>();
        internal RoleFilter(Dictionary<string, bool> roles, ShopDbEntities context)
        {
            Roles = roles;
            using var userStore = new UserStore<User>(context);
            foreach (var key in roles.Keys)
            {
                if(!_usersInRole.ContainsKey(key))
                    _usersInRole.Add(key, Task.Run((() => userStore.GetUsersInRoleAsync(key))).Result);
            }
        }

        public Dictionary<string, bool> Roles { get; }
        public async Task<bool> IsInRolesAsync(User user)
        {
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
            return Task.FromResult(role.Value && _usersInRole[role.Key].Contains(user));
        }
    }
}
