using DynamoDbMigrator.DynamoDbCli.Commands.JsonModel;

namespace DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand
{
    [Option("local-secondary-indexes", OptionType = OptionType.Json)]
    class LocalSecondaryIndexOption
    {
        [Option(OptionType = OptionType.Json)]
        public JsonObject<LocalSecondaryIndexJsonModel> JsonObject { get; set; }
    }
}
