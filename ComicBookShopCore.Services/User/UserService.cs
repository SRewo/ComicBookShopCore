using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Data.User> _manager;
        private readonly IMapper _mapper;

        public UserService(UserManager<Data.User> manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<UserTokenDto> Login(string username, string password)
        {
            var user = await _manager.FindByNameAsync(username);

            if (user == null) return null;

            var result = await _manager.CheckPasswordAsync(user, password);
            if (!result)
                 return null;


            var dto = _mapper.Map<UserTokenDto>(user);
            var roles = await _manager.GetRolesAsync(user);
            dto.Role = roles.First();
            return dto;
        }

        public async Task Register(UserRegisterDto userDto)
        {
            if (userDto.Password != userDto.ConfirmPassword)
                throw new ValidationException("The password and confirmation password do not match.");

            var user = _mapper.Map<Data.User>(userDto);

            var result = await _manager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                var errors = new StringBuilder();
                foreach (var identityError in result.Errors) errors.AppendLine(identityError.Description);
                throw new ValidationException(errors.ToString());
            }
        }

    }
}