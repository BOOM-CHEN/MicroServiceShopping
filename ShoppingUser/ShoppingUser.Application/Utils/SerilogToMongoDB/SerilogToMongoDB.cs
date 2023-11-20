using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ShoppingUser.Application.Utils.SerilogToMongoDB
{
    public static class SerilogToMongoDB
    {
        private static readonly WebApplicationBuilder builder;
        static SerilogToMongoDB()
        {
            builder = WebApplication.CreateBuilder();
        }
        /// <summary>
        /// 存储日志
        /// </summary>
        /// <param name="services"></param>
        public static void AddSerilogToMongoDBService(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            var MongoDBConnection = builder.Configuration.GetSection("MongoDB").Get<MongoDBSetting>();
            var MongoDBClient = new MongoClient(MongoDBConnection.ConnectionString);
            var MongoDBDataBase = MongoDBClient.GetDatabase(MongoDBConnection.DatabaseName);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.MongoDB(
                    MongoDBDataBase,
                    collectionName: MongoDBConnection.CollectionName,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    period: TimeSpan.FromSeconds(1),//批量写入的时间
                    batchPostingLimit: 10,//每次批量写入的数量
                    mongoDBJsonFormatter: new CustomFormatProvider()
                )
                .CreateLogger();
        }
    }
}
