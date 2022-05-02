using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamoDb;

namespace DynamoDbMigrator
{
    class BaseHandler
    {

        protected IDynamoDbContext FindAndCreateContext(string path, DynamoDbSettings dbSettings)
        {
            Assembly assembly = Assembly.LoadFrom(path);

            Type type = assembly.GetTypes().Where(t => t.BaseType == typeof(DynamoDbContext)).FirstOrDefault();
            if (type is null)
                throw new TypeLoadException($"The assembly {Path.GetFileName(path)} does not contain any context class that implements {nameof(DynamoDbContext)}");

            IDynamoDbContext dynamoDbContext = (IDynamoDbContext) Activator.CreateInstance(type, new object[] { dbSettings });
            if (dynamoDbContext is null)
            {
                throw new TypeLoadException();
            }

            return dynamoDbContext;
        }
    }
}
