using DynamoDbORM.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbORM.Persistence.DynamoDb
{
    public interface ICommonRepository<T> where T : BaseEntity
    {
        Task<T> GetItemAsync(string hash, string range);
        Task<T> GetItemByNameAsync(string name, bool isHash = false);
        Task<string> AddItemAsync(T item);
        Task UpdateItemAsync(string hash, string range, T item);
        Task<bool> DeleteItem(string hash, string range);
        Task<bool> CheckItemExists(string hash, string range);
    }
}
