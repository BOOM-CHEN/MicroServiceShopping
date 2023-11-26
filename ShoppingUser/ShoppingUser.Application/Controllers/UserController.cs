using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using ShoppingUser.Application.IService;
using ShoppingUser.Application.Utils;
using ShoppingUser.EntityModel.Dto;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService , IMapper mapper,IDistributedCache distributedCache)
        {
            _userService = userService;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [Authorize(Roles = ("root"))]
        [HttpGet("GetAllUser")]
        public async Task<MessageModel<List<UserInfoDto>>> GetAllUser()
        {
            var cache = await _distributedCache.GetStringAsync("GetAllUser");
            if(cache == null)
            {
                var users = await _userService.FindAllUserAsync();
                var usersInfo = _mapper.Map<List<UserInfoDto>>(users);
                await _distributedCache.SetStringAsync("GetAllUser", JsonConvert.SerializeObject(usersInfo),new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(1),SlidingExpiration = TimeSpan.FromSeconds(30)});
                cache = await _distributedCache.GetStringAsync("GetAllUser");
            }
            
            return new MessageModel<List<UserInfoDto>>()
            {
                status = 200,
                success = true,
                data = JsonConvert.DeserializeObject<List<UserInfoDto>>(cache)
            };
        }

        [HttpGet("GetUserById")]
        public async Task<MessageModel<UserInfoDto>> GetUserById(Guid id)
        {
            var user = await _userService.FindUserAsync(u => u.Id == id);
            if(user == null)
            {
                return new MessageModel<UserInfoDto>()
                {
                    status = 404,
                    success = false,
                    message = "查无此人"
                };
            }
            var userInfo = _mapper.Map<UserInfoDto>(user);
            return new MessageModel<UserInfoDto>()
            {
                status = 200,
                success = true,
                data = userInfo
            };
        }

        [HttpGet("GetUserByEmail")]
        public async Task<MessageModel<UserInfoDto>> GetUserByEmail(string email)
        {
            var user = await _userService.FindUserAsync(u => u.UserEmail == email);
            if(user == null)
            {
                return new MessageModel<UserInfoDto>()
                {
                    status = 404,
                    success = false,
                    message = "查无此人"
                };
            }
            var userInfo = _mapper.Map<UserInfoDto>(user);
            return new MessageModel<UserInfoDto>()
            {
                status = 200,
                success = true,
                data = userInfo
            };
        }

        [Authorize(Roles = ("root"))]
        [HttpGet("GetLimitAllUser")]
        public async Task<MessageModel<List<UserInfoDto>>> GetLimitAllUser(int skip,int take)
        {
            var users = await _userService.FindLimitListUserAsync(skip, take);
            var usersInfo = _mapper.Map<List<UserInfoDto>>(users);
            return new MessageModel<List<UserInfoDto>>()
            {
                status = 200,
                success = true,
                data = usersInfo
            };
        }

        [Authorize(Roles = "root")]
        [HttpGet("GetLimitRoleUser")]
        public async Task<MessageModel<List<UserInfoDto>>> GetLimitRoleUser(string role,int skip,int take)
        {
            if(role != "user" && role != "admin" && role != "root")
            {
                return new MessageModel<List<UserInfoDto>>()
                {
                    status = 404,
                    success = false,
                    message = "不存在该角色"
                };
            }
            var users = await _userService.FindLimitListUserAsync(u => u.Role == role,skip,take);
            var usersInfo = _mapper.Map<List<UserInfoDto>>(users);
            return new MessageModel<List<UserInfoDto>>()
            {
                status = 200,
                success = true,
                data = usersInfo
            };
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetLimitUserUser")]
        public async Task<MessageModel<List<UserInfoDto>>> GetLimitUserUser(int skip, int take)
        {
            var users = await _userService.FindLimitListUserAsync(u => u.Role == "user", skip, take);
            var usersInfo = _mapper.Map<List<UserInfoDto>>(users);
            return new MessageModel<List<UserInfoDto>>()
            {
                status = 200,
                success = true,
                data = usersInfo
            };
        }

        [Authorize(Roles = "root")]
        [HttpPost("AddUser")]
        public async Task<MessageModel<string>> AddUser(AddUserDto addUserDto)
        {
            if(addUserDto.Role != "admin" && addUserDto.Role != "user" && addUserDto.Role != "root")
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "角色有误"
                };
            }
            if (!EmailConfirm.ConfirmEmail(addUserDto.UserEmail))
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "邮箱无效"
                };
            }
            var isExist = await _userService.FindUserAsync(u => u.UserEmail == addUserDto.UserEmail);
            if (isExist != null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该账号已存在"
                };
            }
            var userPhoneNumber = await _userService.FindUserAsync(u => u.UserPhoneNumber == addUserDto.UserPhoneNumber);
            if (userPhoneNumber != null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该电话已被绑定"
                };
            }
            var user = _mapper.Map<User>(addUserDto);
            byte[] cipherText = PasswordAES.Encrypt(addUserDto.UserPassword, out byte[] PublicKey, out byte[] IV);
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
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                message = "添加成功!",
                success = true
            };
        }

        [Authorize(Roles = "admin")]
        [HttpPost("AddUserUser")]
        public async Task<MessageModel<string>> AddUserUser(AddUserDto addUserDto)
        {
            if (addUserDto.Role != "user")
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "角色有误"
                };
            }
            if (!EmailConfirm.ConfirmEmail(addUserDto.UserEmail))
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "邮箱无效"
                };
            }
            var isExist = await _userService.FindUserAsync(u => u.UserEmail == addUserDto.UserEmail);
            if (isExist != null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该账号已存在"
                };
            }
            var userPhoneNumber = await _userService.FindUserAsync(u => u.UserPhoneNumber == addUserDto.UserPhoneNumber);
            if (userPhoneNumber != null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该电话已被绑定"
                };
            }
            var user = _mapper.Map<User>(addUserDto);
            byte[] cipherText = PasswordAES.Encrypt(addUserDto.UserPassword, out byte[] PublicKey, out byte[] IV);
            user.UserPassword = Convert.ToBase64String(cipherText);
            user.RegisterTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.Role = "user";
            user.Passwords = new()
            {
                Id = new Guid(),
                UserId = user.Id,
                PublicKey = Convert.ToBase64String(PublicKey),
                IV = Convert.ToBase64String(IV)
            };
            await _userService.InsertUserAsync(user);
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                message = "添加成功!",
                success = true
            };
        }

        [Authorize(Roles = "root")]
        [HttpPut("UpdateUser")]
        public async Task<MessageModel<string>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _userService.FindUserAsync(u => u.Id == updateUserDto.Id);
            if(user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "未找到用户"
                };
            }
            var userEmail = await _userService.FindUserAsync(u => u.UserEmail == updateUserDto.UserEmail);
            if(updateUserDto.UserEmail != "" && userEmail != null && userEmail.UserEmail != user.UserEmail)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该邮箱已被注册"
                };
            }
            var userPhoneNumber = await _userService.FindUserAsync(u => u.UserPhoneNumber == updateUserDto.UserPhoneNumber);
            if (updateUserDto.UserPhoneNumber != "" && userPhoneNumber != null && userPhoneNumber.UserPhoneNumber != user.UserPhoneNumber)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该电话已被绑定"
                };
            }
            if(updateUserDto.Role != "" && updateUserDto.Role != "root" && updateUserDto.Role != "admin" && updateUserDto.Role != "user")
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "角色不存在"
                };
            }
            await _userService.UpdateUserAsync(u => u.Id == updateUserDto.Id,
                s => s.SetProperty(sp => sp.UserName, updateUserDto.UserName ==""  ? user.UserName : updateUserDto.UserName)
                      .SetProperty(sp => sp.UserEmail,updateUserDto.UserEmail == "" ? user.UserEmail : updateUserDto.UserEmail)
                      .SetProperty(sp => sp.UserPhoneNumber, updateUserDto.UserPhoneNumber == "" ? user.UserPhoneNumber : updateUserDto.UserPhoneNumber)
                      .SetProperty(sp => sp.UserRecieveAddress, updateUserDto.UserRecieveAddress == "" ? user.UserRecieveAddress : updateUserDto.UserRecieveAddress)
                      .SetProperty(sp => sp.Role, updateUserDto.Role == "" ? user.Role : updateUserDto.Role)
                );
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "更新成功"
            };
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("UpdateUserUser")]
        public async Task<MessageModel<string>> UpdateUserUser(UpdateUserUserDto updateUserUserDto)
        {
            var user = await _userService.FindUserAsync(u => u.Id == updateUserUserDto.Id);
            if (user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "未找到用户"
                };
            }
            var userEmail = await _userService.FindUserAsync(u => u.UserEmail == updateUserUserDto.UserEmail);
            if (updateUserUserDto.UserEmail != "" && userEmail != null && userEmail.UserEmail != user.UserEmail)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该邮箱已被注册"
                };
            }
            var userPhoneNumber = await _userService.FindUserAsync(u => u.UserPhoneNumber == updateUserUserDto.UserPhoneNumber);
            if (updateUserUserDto.UserPhoneNumber != "" && userPhoneNumber != null && userPhoneNumber.UserPhoneNumber != user.UserPhoneNumber)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "该电话已被绑定"
                };
            }
            await _userService.UpdateUserAsync(u => u.Id == updateUserUserDto.Id,
                s => s.SetProperty(sp => sp.UserName, updateUserUserDto.UserName == "" ? user.UserName : updateUserUserDto.UserName)
                      .SetProperty(sp => sp.UserEmail, updateUserUserDto.UserEmail == "" ? user.UserEmail : updateUserUserDto.UserEmail)
                      .SetProperty(sp => sp.UserPhoneNumber, updateUserUserDto.UserPhoneNumber == "" ? user.UserPhoneNumber : updateUserUserDto.UserPhoneNumber)
                      .SetProperty(sp => sp.UserRecieveAddress, updateUserUserDto.UserRecieveAddress == "" ? user.UserRecieveAddress : updateUserUserDto.UserRecieveAddress)
                );
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "更新成功"
            };
        }

        [HttpPut("UpdateUserPassword")]
        public async Task<MessageModel<string>> UpdateUserPassword(Guid id,string password)
        {
            var user = await _userService.FindUserAsync(u => u.Id == id);
            if(user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "用户不存在"
                };
            }
            byte[] ciphertext = PasswordAES.Encrypt(password,out byte[] PublicKey,out byte[] IV);
            await _userService.UpdateUserAsync(u => u.Id == id, s => s.SetProperty(sp => sp.UserPassword, Convert.ToBase64String(ciphertext)));
            await _userService.UpdatePasswordAsync(p => p.UserId == user.Id,
                s => s.SetProperty(sp => sp.PublicKey, Convert.ToBase64String(PublicKey))
                      .SetProperty(sp => sp.IV,Convert.ToBase64String(IV)));
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "密码更新成功"
            };
        }

        [Authorize(Roles = "root")]
        [HttpDelete("DeleteAllUser")]
        public async Task<MessageModel<string>> DeleteAllUser()
        {
            await _userService.DeleteAllUserAsync();
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "用户已全部清空"
            };
        }

        [HttpDelete("DeleteUserById")]
        public async Task<MessageModel<string>> DeleteUserById(Guid id)
        {
            var user = await _userService.FindUserAsync(u => u.Id == id);
            if(user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "用户不存在"
                };
            }
            await _userService.DeleteUserAsync(u => u.Id == id);
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "删除成功"
            };
        }

        [Authorize(Roles = "admin,root")]
        [HttpDelete("DeleteUserByEmail")]
        public async Task<MessageModel<string>> DeleteUserByEmail(string email)
        {
            var user = await _userService.FindUserAsync(u => u.UserEmail == email);
            if (user == null)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "用户不存在"
                };
            }
            await _userService.DeleteUserAsync(u => u.UserEmail == email);
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = "删除成功"
            };

        }

        [Authorize(Roles = "admin,root")]
        [HttpDelete("DeleteManyUser")]
        public async Task<MessageModel<string>> DeleteManyUser([FromBody] List<string> emails)
        {
            if(emails.Count == 0)
            {
                return new MessageModel<string>()
                {
                    status = 404,
                    success = false,
                    message = "请输入要删除的邮箱"
                };
            }
            int num = 0;
            for(int count = 0; count < emails.Count; count++)
            {
                var user = await _userService.FindUserAsync(u => u.UserEmail == emails[count]);
                if(user == null)
                {
                    continue;
                }
                await _userService.DeleteUserAsync(u => u.UserEmail == emails[count]);
                num++;
            }
            await _distributedCache.RemoveAsync("GetAllUser");
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                message = $"已删除{num}条,{emails.Count - num}条删除失败"
            };
        }

    }
}
