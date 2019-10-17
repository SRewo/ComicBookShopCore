using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.User
{
    public interface IUserService
    {
        Task<UserTokenDto> Login(string username, string password);
        Task<IDictionary<string,string>> Register(UserRegisterDto userDto, string role);
        Task<IEnumerable<UserDto>> UserList();
        Task<UserDto> FindUserById(string id);
        Task<UserDto> FindUserByUserName(string userName);
        Task<UserUpdateDto> UserForUpdate(string id);
        Task<IDictionary<string, string>> UpdateUserInfo(string id,UserUpdateDto userDto);
        Task<IDictionary<string,string>> UpdatePassword(string id, ChangePasswordDto passwordDto);
    }
}