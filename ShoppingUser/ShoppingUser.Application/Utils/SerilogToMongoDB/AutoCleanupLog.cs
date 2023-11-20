using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Utils.SerilogToMongoDB
{
    public class AutoCleanupLog
    {
        private Timer _timer;
        private readonly TimeSpan _cleanupInterval;
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private int listenCount = 0;
        /// <summary>
        /// 定时删除日志
        /// </summary>
        /// <param name="cleanupInterval"></param>
        /// <param name="mongoCollection"></param>
        public AutoCleanupLog(TimeSpan cleanupInterval, IMongoCollection<BsonDocument> mongoCollection)
        {
            _cleanupInterval = cleanupInterval;
            _mongoCollection = mongoCollection;
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            // 创建定时器，指定回调方法和触发间隔
            _timer = new Timer(CleanupLogs, null, TimeSpan.Zero, _cleanupInterval);
            Console.WriteLine("开始监听");
        }

        private void CleanupLogs(object state)
        {
            DateTime ExpireTime;
            int deletedCount = 0;
            /*if (!test.Any())
            {
                Console.WriteLine("日志为空,停止监听");
                Stop();
                return;
            }*/

            var res = _mongoCollection.Find(_ => true).ToEnumerable();
            foreach (var item in res)
            {
                ExpireTime = DateTime.ParseExact(item["ExpireTime"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                if ((ExpireTime - DateTime.Now).TotalSeconds <= 0)
                {
                    _mongoCollection.DeleteOne(item);
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                Console.WriteLine($"{deletedCount}条日志已过期");
            }

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 第" + ++listenCount + "次监听");
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            // 停止定时器
            _timer?.Change(Timeout.Infinite, 0);
        }
    }
}
