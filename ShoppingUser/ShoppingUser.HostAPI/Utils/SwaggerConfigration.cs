using Microsoft.OpenApi.Models;

namespace ShoppingUser.HostAPI.Utils
{
    public static class SwaggerConfigration
    {
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(configure =>
            {

                configure.SwaggerDoc("v1",new OpenApiInfo { Title = "ShoppingUser" , Version = "v1" });

                #region 添加安全定义
                configure.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入Token,格式为Bearer XXX",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                #endregion

                #region 添加安全要求
                configure.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                            
                        },new string[] {}
                    }
                });
                #endregion
            });
        }
    }
}
