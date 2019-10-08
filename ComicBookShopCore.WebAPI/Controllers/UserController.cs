﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
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
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
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