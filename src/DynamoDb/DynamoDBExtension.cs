using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb
{
    public static class DynamoDBExtension
    {
        public static string GetTableName(this Type tType)
        {
            var tableClassAtrr = tType.GetCustomAttributesData().FirstOrDefault();
            string tableName= tableClassAtrr?.ConstructorArguments.FirstOrDefault().Value.ToString();
            if(string.IsNullOrEmpty(tableName))
            {
                return tType.Name;
            }
            return tableName;
        }



    }
}
