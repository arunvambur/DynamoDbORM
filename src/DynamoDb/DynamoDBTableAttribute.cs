using System;

namespace DynamoDb
{
    public class DynamoDBTableAttribute: Attribute
    {
        public string TableName { get; set; }

        public DynamoDBTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
