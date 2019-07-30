using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Data.Filters
{
    public interface IUserEmployeeFilterFactory
    {
        Task<IRoleFilter> CheckEmployeeAsync(bool isEmployee);
        Task<IRoleFilter> CheckUserAsync(bool isUser);
        Task<IRoleFilter> CheckEmployeeOrUserAsync(bool isEmployee, bool isUser);
    }

    public class DbRoleFilterFactory : IUserEmployeeFilterFactory
    {
        private readonly ShopDbEntities _context;

        public DbRoleFilterFactory(ShopDbEntities context)
        {
            _context = context;
        }

        public Task<IRoleFilter> CheckEmployeeAsync(bool isEmployee)
        {
            var dictionary = new Dictionary<string, bool> {{"EMPLOYEE", isEmployee}};

            return Task.FromResult((IRoleFilter) new RoleFilter(dictionary, _context));
        }

        public Task<IRoleFilter> CheckUserAsync(bool isUser)
        {
            var dictionary = new Dictionary<string, bool> {{"USER", isUser}};

            return Task.FromResult((IRoleFilter) new RoleFilter(dictionary, _context));
        }

        public Task<IRoleFilter> CheckEmployeeOrUserAsync(bool isEmployee, bool isUser)
        {
            var dictionary = new Dictionary<string, bool>{{"EMPLOYEE",isEmployee}, {"USER", isUser}};

            return Task.FromResult((IRoleFilter) new RoleFilter(dictionary, _context));
        }
    }
}
