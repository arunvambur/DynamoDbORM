using System.Threading;
using System.Threading.Tasks;

namespace DynamoDb
{
    public interface IDynamoDbTableTransaction<TTable>
    {
        /// <summary>
        /// Create a new table in dynamo db
        /// </summary>
        /// <returns>Creation status true or false</returns>
        /// <exception cref="DynamoDbClientException">Failed to create a table</exception>
        Task<bool> CreateTable();

        /// <summary>
        /// Delete a table in dynamod db
        /// </summary>
        /// <param name="tableName">Table to be deleted</param>
        /// <returns>Deletion status true or false</returns>
        /// <exception cref="DynamoDbClientException">Failed to delete a table</exception>
        Task<bool> DeleteTable();

        Task<string> CreateItem(TTable item, CancellationToken cancellationToken = default);
        Task UpdateItem(string hash, string range, TTable item, CancellationToken cancellationToken = default);
        Task DeleteItem(string hash, string range, CancellationToken cancellationToken = default);
        Task<TTable> RetriveItem(string hash, string range);
        Task<TTable> RetriveItemByName(string name, bool isHash = false);
        Task<bool> CheckItemExists(string hash, string range);
    }
}
