using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using DynamoDbMigrator.Commands;
using DynamoDb;

namespace DynamoDbMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<MigrateOption, ScriptOption>(args)
                .WithParsed<MigrateOption>(option => RunMigrate(option))
                .WithParsed<ScriptOption>(option => RunScript(option))
                .WithNotParsed(errors => Console.WriteLine("Arguments passed is not valid, Use -h or --help to get the arguments documentation "));

        }

        private static void RunMigrate(MigrateOption option)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Path = {option.Path}");
            if (!string.IsNullOrEmpty(option.Context)) sb.Append($", Context = {option.Context}");
            sb.Append($", Url = {option.Url}").Append($", Recreate = {option.Recreate}");

            Console.WriteLine($"Arguments passed [{sb.ToString()}]");

            try
            {
                //Validate path
                if (!Path.IsPathFullyQualified(option.Path)) throw new FormatException($"The path '{option.Path}' is not valid");

                Migrator migrator = null;
                if (!string.IsNullOrEmpty(option.Url))
                {
                    //Parse url
                    Uri uri = new Uri(option.Url);

                    DynamoDbSettings dbSettings = new DynamoDbSettings
                    {
                        EndpointUrl = uri.AbsoluteUri,
                        Port = uri.Port,
                        UseLocalDatabase = true
                    };

                    migrator = new Migrator(option.Path, dbSettings, option.Recreate);
                }
                else
                {
                    DynamoDbSettings dbSettings = new DynamoDbSettings
                    {
                        UseLocalDatabase = false
                    };
                    migrator = new Migrator(option.Path, dbSettings, option.Recreate);
                }



                Task t = migrator.Migrate();
                t.Wait();
                Console.WriteLine("Dynamo DB Created Sucessfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private static void RunScript(ScriptOption option)
        {
            try
            {
                //Validate path
                if (!Path.IsPathFullyQualified(option.Path)) throw new FormatException($"The path '{option.Path}' is not valid");

                if (!string.IsNullOrEmpty(option.Url))
                {
                    //Parse url
                    Uri uri = new Uri(option.Url);

                    DynamoDbSettings dbSettings = new DynamoDbSettings
                    {
                        EndpointUrl = uri.AbsoluteUri,
                        Port = uri.Port,
                        UseLocalDatabase = true
                    };

                    ScriptGenerator script = new ScriptGenerator(option.Path, option.Type, option.Output, recreateTables: option.Recreate, url: dbSettings.EndpointUrl);
                    script.Generate();
                }
                else
                {
                    DynamoDbSettings dbSettings = new DynamoDbSettings
                    {
                        UseLocalDatabase = false
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }
    }
}
