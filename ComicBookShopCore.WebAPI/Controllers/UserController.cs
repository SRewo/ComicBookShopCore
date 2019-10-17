using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ComicBookShopCore.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace ComicBookShopCore.WebAPI.Controllers
{
    //TODO: User Update/Change password
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/userList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult<IQueryable<UserDto>>> GetUserList()
        {
            var result = await _service.UserList();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("api/user")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetLoggedUserInfo()
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid);

            if (idClaim == null)
                return BadRequest();

            var user = await _service.FindUserById(idClaim.Value);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("api/user/id/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            if (id == null)
                return BadRequest();

            var user = await _service.FindUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("api/user/{userName}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult<UserDto>> GetUserByUserName(string userName)
        {
            if (userName == null)
                return BadRequest();

            var user = await _service.FindUserByUserName(userName);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetToken([FromBody] UserLoginDto user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) )
                return BadRequest("Login/password cannot be null or empty");

            var userDto = await _service.Login(user.Login, user.Password);

            if (userDto == null)
                return BadRequest("Invalid login/password.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("TX0BudRrGZ37Ymwi7mnf");
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.PrimarySid, userDto.Id.ToString()),
                    new Claim(ClaimTypes.Name, userDto.Login),
                    new Claim(ClaimTypes.Role, userDto.Role)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDesc);

            return Ok(tokenHandler.WriteToken(token));
        }

        [HttpPost]
        [Route("api/register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto user)
        {
            if (user == null)
                return BadRequest();

            var result = await _service.Register(user, "User");

            if (result == null)
                return Ok();

            var state = new ModelStateDictionary();

            foreach (var (key, value) in result) state.AddModelError(key, value);

            return ValidationProblem(new ValidationProblemDetails(state));
        }

        [HttpPost]
        [Route("api/employees/register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        public async Task<ActionResult> RegisterEmployee([FromBody] UserRegisterDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _service.Register(user, "Employee");

            if (result == null)
                return Ok();

            var state = new ModelStateDictionary();

            foreach (var (key, value) in result) state.AddModelError(key, value);

            return ValidationProblem(new ValidationProblemDetails(state));
        }
    }
}