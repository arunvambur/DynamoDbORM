using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.JsonModel
{
    class LocalSecondaryIndexJsonModel
    {
        public string IndexName { get; set; }

        public List<KeySchemaElementJsonModel> KeySchema { get; set; }

        public ProjectionJsonModel Projection { get; set; }
    }
}
