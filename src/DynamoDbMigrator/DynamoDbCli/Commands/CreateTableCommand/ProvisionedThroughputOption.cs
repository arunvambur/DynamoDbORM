using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand
{
    [Option("provisioned-throughput", OptionType = OptionType.KeyValue)]
    class ProvisionedThroughputOption
    {
        [Option]
        public string ReadCapacityUnits { get; set; }

        [Option]
        public string WriteCapacityUnits { get; set; }
    }
}
