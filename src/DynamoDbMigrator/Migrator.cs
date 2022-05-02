using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DynamoDb;

namespace DynamoDbMigrator
{
    class Migrator : BaseHandler
    {
        readonly string _path;
        readonly string _context;
        readonly DynamoDbSettings _dbSettings;
        readonly bool _recreateTables;

        public Migrator(string path, DynamoDbSettings dbSettings, bool recreateTables = false)
        {
            _path = path;
            _dbSettings = dbSettings;
            _recreateTables = recreateTables;
        }

        public Migrator(string path, DynamoDbSettings dbSettings, string context, bool recreateTables = false) : this(path, dbSettings, recreateTables)
        {
            _context = context;
        }

        public async Task Migrate()
        {
            IDynamoDbContext dynamoDbContext = FindAndCreateContext(_path, _dbSettings);

            if(_recreateTables)
            {
                await dynamoDbContext.DeleteTableModel();
            }

             await dynamoDbContext.CreateTableModel();

            await dynamoDbContext.DeployTableModel();

            //Todo:Handle error when there is no connectivity to aws dynamo db

            //Todo: Handle recreate option to remove the existing table and re-create it again
        }
    }
}
