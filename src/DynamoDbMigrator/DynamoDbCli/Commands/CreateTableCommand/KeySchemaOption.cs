using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand
{
    [Option(OptionType = OptionType.KeyValue)]
    class KeySchemaOption
    {
        [Option]
        public string AttributeName { get; set; }

        [Option]
        public string KeyType { get; set; }
    }
}
