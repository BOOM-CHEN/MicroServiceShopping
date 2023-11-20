using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Application.IServices;
using Shopping.Application.Utils;
using Shopping.Entity.Dto;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Application.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<MessageModel<string>> Login(LoginDto loginDto)
        {
            var user = await _userService.FindUserAsync(x => x.UserEmail == loginDto.UserEmail);
            if (user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "账号或密码错误",
                };
            }
            var password = await _userService.FindPasswordAsync(x => x.UserId == user.Id);
            var p = PasswordAES.Decrypt(
                Convert.FromBase64String(user.UserPassword),
                Convert.FromBase64String(password.PublicKey),
                Convert.FromBase64String(password.IV)
            );
            if(p != loginDto.UserPassword)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "账号或密码错误",
                };
            }
            //判断Jwt是否过期
            if (!JwtHelper.ValidateToken(user.Token))
            {
                user.Token = JwtHelper.CreateJwt(user.Role, user.UserName, user.UserEmail);
                await _userService.UpdateUserAsync(user);
            }

            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "登录成功",
                data = user.Token
            };
        }

        [HttpPost("Register")]
        public async Task<MessageModel<string>> Register(RegisterDto registerDto)
        {
            if(string.IsNullOrEmpty(registerDto.UserPassword))
            {
                return new MessageModel<string>
                {
                    status = 404,
                    message = "请输入密码",
                    success = false
                };
            }
            if (!EmailConfirm.ConfirmEmail(registerDto.UserEmail))
            {
                return new MessageModel<string>
                {
                    status = 404,
                    message = "请输入正确的邮箱",
                    success = false
                };
            }
            var isExistUser = await _userService.FindUserAsync(u => u.UserEmail == registerDto.UserEmail);
            if(isExistUser != null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "用户已存在"
                };
            }
            var user = _mapper.Map<User>(registerDto);
            byte[] cipherText = PasswordAES.Encrypt(user.UserPassword,out byte[] PublicKey,out byte[] IV);
            user.Role = "user";
            user.UserPassword = Convert.ToBase64String(cipherText);
            user.RegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.Passwords = new()
            {
                Id = new Guid(),
                UserId = user.Id,
                PublicKey = Convert.ToBase64String(PublicKey),
                IV = Convert.ToBase64String(IV)
            };
            await _userService.InsertUserAsync(user);
            return new MessageModel<string>()
            {
                status = 200,
                message = "注册成功!",
                success = true
            };
        }
    }
}
