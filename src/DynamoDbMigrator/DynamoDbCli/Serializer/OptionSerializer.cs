using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DynamoDbMigrator.DynamoDbCli.Commands;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    class OptionSerializer : ISerializer
    {
        public string Serialize(object obj, FormatCommand formatCommand)
        {
            if (obj is null) return "";
            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            var attribute = t.GetCustomAttribute<OptionAttribute>();
            if (attribute is not null && !string.IsNullOrEmpty(attribute.Name))
            {
                sb.Append("--").Append(attribute.Name).Append(" ");
            }

            var properties = t.GetProperties();
            foreach (var prop in properties)
            {
                var attri = prop.GetCustomAttribute<OptionAttribute>();

                if (attri is not null)
                {
                    if (attribute.OptionType == OptionType.KeyValue)
                    {
                        var value = prop.GetValue(obj);
                        if (prop.PropertyType == typeof(string))
                        {
                            sb.Append(string.IsNullOrEmpty(attri.Name) ? prop.Name : attri.Name).Append("=").Append(value).Append(",");
                        }
                    }
                    else if(attribute.OptionType == OptionType.Json)
                    {
                        IJsonObject value =(IJsonObject) prop.GetValue(obj);
                        sb.Append(" ").Append("\"").Append(value.GetJson()).Append("\"");
                    }
                }
            }            

            return sb.ToString().TrimEnd(',');
        }
    }
}
