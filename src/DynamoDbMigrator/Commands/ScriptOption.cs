using CommandLine;

namespace DynamoDbMigrator.Commands
{
    [Verb("script", HelpText = "Generate script for aws dynamodb from context model")]
    public class ScriptOption : CommonOption
    {
        [Option("type", Required = true, HelpText = "Type of script. Use 'ps' for powershell and 'sh' for bash script")]
        public string Type { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output path for script file")]
        public string Output { get; set; }

        [Option('u', "url", HelpText = "Dynamodb url")]
        public string Url { get; set; }

    }
}
