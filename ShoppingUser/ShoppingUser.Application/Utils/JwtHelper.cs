using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingUser.HostAPI.Utils
{
    public static class JwtHelper
    {
        private const string Secretkey = "$7mV6u9GhD1_W*MpD&t+AuBoT7I8+UAz_HFm3D_355C(1RVDw";
        public static string CreateJwt(string role, string username, string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secretkey);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Email,email),
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                //Expires = DateTime.Now.AddDays(1),
                //Expires = DateTime.Now.AddHours(2),
                Expires = DateTime.Now.AddSeconds(20),
                //Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        // 从 JWT 中提取 ClaimsPrincipal 对象
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Secretkey);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out _);
                return principal;
            }
            catch
            {
                // 无效的令牌
                return null;
            }
        }

        // 验证 JWT 的有效性
        public static bool ValidateToken(string token)
        {
            var principal = GetPrincipal(token);
            if (principal == null)
                return false;

            var identity = principal.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            // 如果需要，您可以进一步检查声明

            return true;
        }
    }
}
