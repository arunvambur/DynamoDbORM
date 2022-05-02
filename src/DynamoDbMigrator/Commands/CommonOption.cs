using CommandLine;

namespace DynamoDbMigrator.Commands
{
    public class CommonOption 
    {
        [Option('p', "path", Required = true, HelpText = "Path to dynamo db context assembly dll file")]
        public string Path { get; set; }

        [Option('c', "context", Required = false, HelpText = "Dynamodb context class")]
        public string Context { get; set; }

        [Option('f', "force-recreate", Required = false, Default = false, HelpText = "Remove existing tables and recreate again")]
        public bool Recreate { get; set; }
    }
}
