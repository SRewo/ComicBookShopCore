using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

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
            dto.Role = roles.FirstOrDefault() ?? string.Empty;
            return dto;
        }

        public async Task<IDictionary<string,string>> Register(UserRegisterDto userDto, string role = "User")
        {
            var errors = new Dictionary<string, string>();
            if (userDto.Password != userDto.ConfirmPassword)
                return new Dictionary<string, string>{{"Password","The password and confirmation password do not match."}};

            if(userDto.Address == null)
                return new Dictionary<string, string>{{"Address","Address cannot be null"}};

            var user = _mapper.Map<Data.User>(userDto);

            var contextUser = new ValidationContext(user);
            var contextAddress = new ValidationContext(user.Address);

            var validationResults = new List<ValidationResult>();

            var isUserValid = Validator.TryValidateObject(user, contextUser, validationResults);
            var isAddressValid = Validator.TryValidateObject(user.Address, contextAddress, validationResults);

            if (!isUserValid || !isAddressValid)
            {
                foreach (var identityError in validationResults) errors.Add(identityError.MemberNames.First(),identityError.ErrorMessage);
                return errors;
            }

            var result = await _manager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, role);
                return null;
            }

            foreach (var identityError in result.Errors) errors.Add(identityError.Code, identityError.Description);
            return errors;
        }

    }
}