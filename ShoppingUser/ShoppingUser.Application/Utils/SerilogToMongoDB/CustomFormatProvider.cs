using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Utils.SerilogToMongoDB
{
    public class CustomFormatProvider : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var log = new CustomMongoDBLogField()
            {
                ExpireTime = logEvent.Timestamp.DateTime.AddHours(new Random().NextDouble() * 6).ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = logEvent.Timestamp.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EventLevel = logEvent.Level.ToString(),
                RequestMethod = logEvent.Properties.TryGetValue("RequestMethod", out var requestMethod) ? requestMethod.ToString() : string.Empty,
                RequestPath = logEvent.Properties.TryGetValue("RequestPath", out var requestPath) ? requestPath.ToString() : string.Empty,
                StatusCode = logEvent.Properties.TryGetValue("StatusCode", out var statusCode) ? statusCode.ToString() : string.Empty

            };
            output.Write(JsonConvert.SerializeObject(log));
        }
    }
}
