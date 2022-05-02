using System.Threading.Tasks;

namespace DynamoDb
{
    public interface IDynamoDbContext
    {
        /// <summary>
        /// Create the table models for DynamoDb
        /// </summary>
        /// <returns></returns>
        Task CreateTableModel();

        /// <summary>
        /// Deploys tables in DynamoDb
        /// </summary>
        /// <returns></returns>
        Task DeployTableModel();

        /// <summary>
        /// Delete the tables from DynamoDb
        /// </summary>
        /// <param name="filtertables">Delete only specific tables. On default it will delete all the tables.</param>
        /// <returns></returns>
        Task DeleteTableModel(string[] filtertables = null);
    }
}
