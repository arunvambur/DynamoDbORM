using System;
using System.Collections.Generic;

namespace DynamoDbMigrator.DynamoDbCli.Commands.DeleteTable
{
    [Command("delete-table")]
    class DeleteTableCommand
    {
        [Option("table-name")]
        public string TableName { get; set; }
    }
}
