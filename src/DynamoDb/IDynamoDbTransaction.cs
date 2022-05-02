using System.Threading;
using System.Threading.Tasks;

namespace DynamoDb
{
    public interface IDynamoDbTransaction<T, U>
    {
        Task<string> CreateItemWithTransaction(T item, U uniqueItem, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemWithTransaction(T item, U uniqueItem, CancellationToken cancellationToken = default);
    }
}
