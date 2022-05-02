using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand;
using DynamoDbMigrator.DynamoDbCli.Commands.DeleteTable;

namespace DynamoDbMigrator.DynamoDbCli.Commands
{
    [Command("aws dynamodb")]
    class BaseCommand
    {
        public CreateTable CreateTable { get; set; }

        public DeleteTableCommand DeleteTable { get; set; }

        [Option("endpoint-url")]
        public string Url { get; set; }

        
    }
}
