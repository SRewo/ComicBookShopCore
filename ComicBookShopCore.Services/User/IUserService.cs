using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.User
{
    public interface IUserService
    {
        Task<UserTokenDto> Login(string username, string password);
        Task<IDictionary<string,string>> Register(UserRegisterDto userDto);
    }
}