
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shopping.API.Utils.SerilogToMongoDB;
using Shopping.Application.IServices;
using Shopping.Application.IServices.IBase;
using Shopping.Application.Services;
using Shopping.Application.Services.Base;
using Shopping.Entity.AutoMapper;
using Shopping.Entity.DBContext;
using Shopping.Entity.IRepositories;
using Shopping.Entity.IRepositories.IBaseRepository;
using Shopping.Entity.Models;
using Shopping.Entity.Repositories;
using Shopping.Entity.Repositories.Base;
using System.Text;

namespace Shopping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthorization();

            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            #region MongoDB
            //MongoDB连接
            builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));
            // 创建并启动定时任务
            new AutoCleanupLog(TimeSpan.FromHours(3), GetMongoDBCollection.GetCollection(builder)).Start();
            #endregion

            #region SeriLog
            builder.Host.UseSerilog();
            builder.Services.AddSerilogToMongoDBService();
            #endregion

            #region DI
            builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBaseService<User>, BaseService<User>>();
            builder.Services.AddScoped<IUserService, UserService>();
            #endregion

            #region Cors
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("MyCors", opt =>
                {
                    opt.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
            });
            #endregion

            #region DB
            builder.Services.AddDbContext<ShoppingDBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
                //opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            });
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapperService();
            #endregion

            #region Jwt
            builder.Services
                .AddAuthorization()//开启认证
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//指定授权的渠道
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$7mV6u9GhD1_W*MpD&t+AuBoT7I8+UAz_HFm3D_355C(1RVDw")),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("MyCors");

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllers();
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}