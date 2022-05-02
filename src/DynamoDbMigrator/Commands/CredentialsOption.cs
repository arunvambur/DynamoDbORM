using CommandLine;

namespace DynamoDbMigrator.Commands
{
    public abstract class CredentialsOption : CommonOption
    {
        [Option('a', "access-key", Required = false, HelpText = "AWS access key")]
        public string AccessKey { get; }

        [Option('s', "secret-key", Required = false, HelpText = "AWS secret key")]
        public string SecretKey { get; }

        [Option('t', "token", Required = false, HelpText = "AWS session token")]
        public string Token { get; }
    }
}
