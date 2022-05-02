using System;
using System.Collections.Generic;

namespace DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand
{
    [Serializable]
    [Command("create-table")]
    class CreateTable
    {
        [Option("table-name")]
        public string TableName { get; set; }

        [Option("provisioned-throughput")]
        public ProvisionedThroughputOption ProvisinedThroughPut { get; set; }

        [Option("attribute-definitions", OptionType = OptionType.List)]
        public List<AttributeDefinitionOption> AttributeDefinitions { get; set; }

        [Option("key-schema", OptionType = OptionType.List)]
        public List<KeySchemaOption> KeySchema { get; set; }

        [Option("global-secondary-indexes")]
        public GlobalSecondaryIndexOption GlobalSecondaryIndexs { get; set; }

        [Option("local-secondary-indexes")]
        public LocalSecondaryIndexOption LocalSecondaryIndexs { get; set; }

        public CreateTable()
        {

        }
    }
}
