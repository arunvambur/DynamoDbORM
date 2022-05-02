using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DynamoDbMigrator.DynamoDbCli.Commands
{
    class JsonObject<T> : IJsonObject
    {
        public List<T> Value { get; set; }

        public string GetJson()
        {
            var settings = new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(Value, settings).Replace("\"", "\\\"");
        }
    }
}
