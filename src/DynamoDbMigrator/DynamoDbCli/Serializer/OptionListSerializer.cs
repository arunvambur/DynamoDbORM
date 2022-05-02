using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    class OptionListSerializer : ISerializer
    {
        public string Serialize(object obj, FormatCommand formatCommand = null)
        {
            if (obj is null) return "";

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            var attribute = t.GetCustomAttribute<OptionAttribute>();
            if (attribute is not null )
            {
                sb.Append("--").Append(attribute.Name).Append(" ");
            }

            foreach (var item in (IList)obj)
            {
                var itemType = item.GetType();
                var serializer = SerializerFactory.GetSerializer(itemType);
                sb.Append(serializer.Serialize(item, formatCommand)).Append(" ");

            }

            return sb.ToString();
        }
    }
}
