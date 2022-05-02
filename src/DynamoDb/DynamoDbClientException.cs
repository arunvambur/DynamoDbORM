using System;

namespace DynamoDb
{
    public class DynamoDbClientException : Exception
    {
        public string HashKey { get; set; }
        public string RangeKey { get; set; }
        public string TableName { get; set; }
        public string MessageCode { get; set; }

        public DynamoDbClientException()
        {

        }

        public DynamoDbClientException(string message) : base(message)
        {

        }
        public DynamoDbClientException(string message, Exception exception) : base(message, exception)
        {
        }

        public DynamoDbClientException(string hashKey, string rangeKey, string tableName, string message)
            : base(message)
        {
            this.HashKey = hashKey;
            this.RangeKey = rangeKey;
            this.TableName = tableName;
        }

        public DynamoDbClientException(string message, string tableName)
            : base(message)
        {
            this.TableName = tableName;
        }

        public DynamoDbClientException(string message, string tableName, Exception exception)
            : base(message, exception)
        {
            this.TableName = tableName;
        }

        public DynamoDbClientException(string hashKey, string rangeKey, string tableName, string message, Exception exception) 
            : base(message, exception)
        {
            this.HashKey = hashKey;
            this.RangeKey = rangeKey;
            this.TableName = tableName;
        }
    }
}
