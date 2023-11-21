using ShoppingUser.Application;
using ShoppingUser.EntityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShoppingUser.Application.IService;
using ShoppingUser.Application.IService.IBase;
using ShoppingUser.Application.Service;
using ShoppingUser.Application.Service.Base;
using ShoppingUser.Application.Utils.SerilogToMongoDB;
using ShoppingUser.EntityModel.AutoMapper;
using ShoppingUser.EntityModel.IRepository;
using ShoppingUser.EntityModel.IRepository.IBase;
using ShoppingUser.EntityModel.Models;
using ShoppingUser.EntityModel.Repository;
using ShoppingUser.EntityModel.Repository.Base;
using ShoppingUser.EntityModel.ShoppingUserDbContext;
using System.Text;

namespace ShoppingUser.HostAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region MongoDb
            //MongoDB连接
            builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));
            // 创建并启动定时任务
            new AutoCleanupLog(TimeSpan.FromHours(3), GetMongoDBCollection.GetCollection(builder)).Start();
            #endregion

            #region Serilog
            builder.Host.UseSerilog();
            builder.Services.AddSerilogToMongoDBService();
            #endregion

            #region Redis
            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = builder.Configuration.GetConnectionString("Redis");
                opt.InstanceName = "RedisInstance";
            });
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapperService();
            #endregion

            #region Cors
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("MyCors", cors =>
                {
                    cors.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyOrigin();
                });
            });
            #endregion

            #region DI
            builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            builder.Services.AddScoped<IBaseService<User>, BaseService<User>>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            #endregion

            #region DB
            builder.Services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
                //opt.UseMySQL(builder.Configuration.GetConnectionString("MySql"));
            });
            #endregion

            #region Jwt
            builder.Services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$7mV6u9GhD1_W*MpD&t+AuBoT7I8+UAz_HFm3D_355C(1RVDw")),
                        ClockSkew = TimeSpan.Zero
                    }
                );
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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


            app.MapControllers();

            app.Run();
        }
    }
}