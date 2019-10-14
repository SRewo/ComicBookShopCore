using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            if (userDto.Password != userDto.ConfirmPassword)
                return new Dictionary<string, string>{{"Password","The password and confirmation password do not match."}};

            if(userDto.Address == null)
                return new Dictionary<string, string>{{"Address","Address cannot be null"}};

            var user = _mapper.Map<Data.User>(userDto);

            var userErrors = await ValidateUser(user);

            if (userErrors.Any())
                return userErrors;

            var result = await _manager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, role);
                return null;
            }

            var errors = new Dictionary<string, string>();
            foreach (var identityError in result.Errors) errors.Add(identityError.Code, identityError.Description);
            return errors; 
        }

        public Task<IEnumerable<UserDto>> UserList()
        {
            var users = _manager.Users.Include(x => x.Address).Where(x => x.Roles.Any(z => z.Name == "User"));
            var result = _mapper.ProjectTo<UserDto>(users.AsQueryable());
            return Task.FromResult(result.AsEnumerable());
        }

        public async Task<UserDto> FindUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var user = await _manager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> FindUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var user = await _manager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.UserName == userName);
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserUpdateDto> UserForUpdate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var user = await _manager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<UserUpdateDto>(user);
            return result;
        }

        public async Task<IDictionary<string, string>> UpdateUserInfo(string id, UserUpdateDto userDto)
        {
            var user = await _manager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);

            if(user == null)
                return new Dictionary<string, string>(){{"Id", "User with provided id was not found."}};

            _mapper.Map(userDto, user);

            var userErrors = await ValidateUser(user);

            if (userErrors.Any())
                return userErrors;

            var result = await _manager.UpdateAsync(user);

            if (result.Succeeded)
                return null; 
            
            var errors = new Dictionary<string, string>();
            foreach (var identityError in result.Errors) errors.Add(identityError.Code, identityError.Description);
            return errors; 

        }

        public async Task<IDictionary<string, string>> UpdatePassword(string id, ChangePasswordDto passwordDto)
        {
            if (passwordDto.NewPassword != passwordDto.NewPasswordConfirm)
                return new Dictionary<string, string>{{"Password","The password and confirmation password do not match."}};

            var user = await _manager.FindByIdAsync(id);
            if (user == null)
                return new Dictionary<string, string>(){{"Id", "User with provided id was not found."}};

            var result =
                await _manager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

            if (result.Succeeded)
                return null;
            var errors = new Dictionary<string, string>();
            foreach (var identityError in result.Errors) errors.Add(identityError.Code, identityError.Description);
            return errors; 
        }

        private Task<Dictionary<string, string>> ValidateUser(Data.User user)
        {
            var errors = new Dictionary<string, string>();

            var contextUser = new ValidationContext(user);
            var contextAddress = new ValidationContext(user.Address);

            var validationResults = new List<ValidationResult>();

            var isUserValid = Validator.TryValidateObject(user, contextUser, validationResults);
            var isAddressValid = Validator.TryValidateObject(user.Address, contextAddress, validationResults);

            if (!isUserValid || !isAddressValid)
                foreach (var identityError in validationResults)
                    errors.Add(identityError.MemberNames.First(), identityError.ErrorMessage);

            return Task.FromResult(errors);
        }
    }
}