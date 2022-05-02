using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    class CommandSerializer : ISerializer
    {
        public string Serialize(object obj, FormatCommand formatCommand)
        {
            if (obj is null) return "";

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            var attribute = t.GetCustomAttribute<CommandAttribute>();
            if (attribute is not null)
            {
                sb.Append(attribute.Name).Append(" ");
            }

            var properties = t.GetProperties();
            formatCommand.NextLevel();
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var attri = prop.GetCustomAttribute<OptionAttribute>();
                    if (attri is not null)
                    {
                        var value = (string)prop.GetValue(obj);
                        sb.Append(formatCommand.NextLine()).Append("--").Append(attri.Name).Append(" ").Append(value).Append(" ");
                    }
                }
                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var attri = prop.GetCustomAttribute<OptionAttribute>();
                    if (attri is not null)
                    {
                        var serializer = SerializerFactory.GetSerializer(prop.PropertyType);
                        var value = prop.GetValue(obj);
                        if (value is null) continue;
                        sb.Append(formatCommand.NextLine()).Append("--").Append(attri.Name).Append(" ").Append(serializer.Serialize(value, formatCommand));
                    }
                }
                else
                {
                    var serializer = SerializerFactory.GetSerializer(prop.PropertyType);
                    var value = prop.GetValue(obj);
                    string serializedString = serializer.Serialize(value, formatCommand);
                    if(!string.IsNullOrEmpty(serializedString))
                        sb.Append(formatCommand.NextLine()).Append(serializer.Serialize(value, formatCommand));
                }
            }

            return sb.ToString();
        }
    }
}
