using Amazon.DynamoDBv2.Model;
using DynamoDbMigrator.DynamoDbCli.Commands.JsonModel;

namespace DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand
{
    [Option("global-secondary-indexes", OptionType = OptionType.Json)]
    class GlobalSecondaryIndexOption
    {
        [Option(OptionType = OptionType.Json)]
        public JsonObject<GlobalSecondaryIndexJsonModel> JsonObject { get; set; }
    }
}
