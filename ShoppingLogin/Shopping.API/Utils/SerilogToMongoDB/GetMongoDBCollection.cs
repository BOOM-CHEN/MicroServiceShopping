using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.API.Utils.SerilogToMongoDB
{
    public static class GetMongoDBCollection
    {
        public static IMongoCollection<BsonDocument> GetCollection(WebApplicationBuilder builder)
        {
            var MongoDBConnection = builder.Configuration.GetSection("MongoDB").Get<MongoDBSetting>();//获取配置文件
            var MongoDBClient = new MongoClient(MongoDBConnection.ConnectionString);//从配置文件中获取连接字符串并建立连接
            var MongoDBDataBase = MongoDBClient.GetDatabase(MongoDBConnection.DatabaseName);//获取数据库
            var LogCollection = MongoDBDataBase.GetCollection<BsonDocument>(MongoDBConnection.CollectionName);//获取集合
            return LogCollection;
        }
    }
}
