using DynamoDbORM.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoDb;

namespace DynamoDbORM.Persistence.DynamoDb
{
    public class CommonRepository<T> : ICommonRepository<T> where T : BaseEntity
    {
        private readonly DynamoDbContext<T> _dbContext;

        public CommonRepository(DynamoDbContext<T> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddItemAsync(T item)
        {
            return await _dbContext.CurrentTable.CreateItem(item);
        }

        public async Task<bool> CheckItemExists(string hash, string range)
        {
            return await _dbContext.CurrentTable.CheckItemExists(hash, range);
        }

        public async Task<bool> DeleteItem(string hash, string range)
        {
            await _dbContext.CurrentTable.DeleteItem(hash, range);
            return true;
        }

        public async Task<T> GetItemAsync(string hash, string range)
        {
            return await _dbContext.CurrentTable.RetriveItem(hash, range);
        }

        public async Task<T> GetItemByNameAsync(string name, bool isHash = false)
        {
            return await _dbContext.CurrentTable.RetriveItemByName(name, isHash);
        }

        public async Task UpdateItemAsync(string hash, string range, T item)
        {
            item.SetUpdatedOn();
            await _dbContext.CurrentTable.UpdateItem(hash, range, item);
        }
    }
}
