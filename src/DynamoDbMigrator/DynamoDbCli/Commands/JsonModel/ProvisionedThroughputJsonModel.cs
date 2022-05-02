using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.JsonModel
{
    class ProvisionedThroughputJsonModel
    {
        public long ReadCapacityUnits { get; set; }
        public long WriteCapacityUnits { get; set; }
    }
}
