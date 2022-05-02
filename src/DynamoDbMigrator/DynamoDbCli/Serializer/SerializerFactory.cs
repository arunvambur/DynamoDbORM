using System;
using System.Collections.Generic;
using System.Reflection;

namespace DynamoDbMigrator.DynamoDbCli.Serializer
{
    static class SerializerFactory
    {
        public static ISerializer GetSerializer(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return new OptionListSerializer();
            }
            else
            {
                var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute is not null)
                {
                    return new CommandSerializer();
                }

                var optionAttribute = type.GetCustomAttribute<OptionAttribute>();
                if (optionAttribute is not null)
                {
                    return new OptionSerializer();
                }
            }

            return null;
        }
    }
}
