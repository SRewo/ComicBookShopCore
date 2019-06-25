using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Data.Searchers
{
    public interface IUserEmployeeSearcherFactory
    {
        Task<IRoleSearcher> CheckEmployeeAsync(bool isEmployee);
        Task<IRoleSearcher> CheckUserAsync(bool isUser);
        Task<IRoleSearcher> CheckEmployeeOrUserAsync(bool isEmployee, bool isUser);
    }

    public class DbRoleSearcherFactory : IUserEmployeeSearcherFactory
    {
        private readonly ShopDbEntities _context;

        public DbRoleSearcherFactory(ShopDbEntities context)
        {
            _context = context;
        }

        public Task<IRoleSearcher> CheckEmployeeAsync(bool isEmployee)
        {
            var dictionary = new Dictionary<string, bool> {{"EMPLOYEE", isEmployee}};

            return Task.FromResult((IRoleSearcher) new RoleSearcher(dictionary, _context));
        }

        public Task<IRoleSearcher> CheckUserAsync(bool isUser)
        {
            var dictionary = new Dictionary<string, bool> {{"USER", isUser}};

            return Task.FromResult((IRoleSearcher) new RoleSearcher(dictionary, _context));
        }

        public Task<IRoleSearcher> CheckEmployeeOrUserAsync(bool isEmployee, bool isUser)
        {
            var dictionary = new Dictionary<string, bool>{{"EMPLOYEE",isEmployee}, {"USER", isUser}};

            return Task.FromResult((IRoleSearcher) new RoleSearcher(dictionary, _context));
        }
    }
}
