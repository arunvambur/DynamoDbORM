using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb
{
    public interface IDynamoDbTableOperation
    {
        
        Task<bool> CreateTable();
        Task<bool> DeleteTable();
        Task<bool> IsTableExistAsync();
        Task<TableDescription> GetTableDescription();
    }
}
