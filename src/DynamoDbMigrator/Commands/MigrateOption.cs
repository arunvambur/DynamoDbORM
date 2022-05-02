using CommandLine;

namespace DynamoDbMigrator.Commands
{
    [Verb("migrate", HelpText = "Migrate dynamodb context model to aws dynamodb")]
    public class MigrateOption : CredentialsOption
    {

        [Option('u', "url", HelpText = "Dynamodb url")]
        public string Url { get; set; }

    }
}
