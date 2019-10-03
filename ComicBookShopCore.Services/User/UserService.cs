using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.User
{
    public interface IUserService
    {
        Task<UserTokenDto> Login(string username, string password);
    }

    public class UserService : IUserService
    {
        private UserManager<Data.User> _manager;
        private IMapper _mapper;

        public UserService(UserManager<Data.User> manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<UserTokenDto> Login(string username, string password)
        {
            var user = await _manager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null) return null;

            var result = await _manager.CheckPasswordAsync(user, password);
            if (!result)
                user = null;


            var dto = _mapper.Map<UserTokenDto>(user);
            var roles = await _manager.GetRolesAsync(user);
            dto.Role = roles.First();
            return dto;
        }

    }
}