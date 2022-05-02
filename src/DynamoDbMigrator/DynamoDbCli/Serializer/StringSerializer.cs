using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    class StringSerializer : ISerializer
    {
        public string Serialize(object obj, FormatCommand formatCommand)
        {
            if (obj is null) return "";

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            var attribute = t.GetCustomAttribute<CommandAttribute>();
            if (attribute is not null)
            {
                sb.Append(attribute.Name).Append(" ").Append(obj);
            }

            return sb.ToString();
        }
    }
}
