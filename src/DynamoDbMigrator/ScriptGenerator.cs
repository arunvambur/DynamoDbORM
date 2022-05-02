using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoDbMigrator.DynamoDbCli;
using DynamoDbMigrator.DynamoDbCli.Commands;
using DynamoDbMigrator.DynamoDbCli.Commands.CreateTableCommand;
using DynamoDbMigrator.DynamoDbCli.Commands.DeleteTable;
using DynamoDbMigrator.DynamoDbCli.Serializer;
using DynamoDb;
using System.Reflection;
using DynamoDbMigrator.DynamoDbCli.Commands.JsonModel;
using DynamoDbMigrator.Writer;

namespace DynamoDbMigrator
{
    class ScriptGenerator : BaseHandler
    {
        readonly string _path;
        readonly string _context;
        readonly bool _recreateTables;
        readonly string _url;
        readonly string _scriptType;
        readonly string _outputPath;

        public ScriptGenerator(string path, string scriptType, string outputPath, bool recreateTables = false, string url = "")
        {
            _path = path;
            _recreateTables = recreateTables;
            _url = url;
            _scriptType = scriptType;
            _outputPath = outputPath;
        }

        public ScriptGenerator(string path, string scriptType, string outputPath, string context, bool recreateTables = false, string url = "") : this(path, scriptType, outputPath, recreateTables, url)
        {
            _context = context;
        }

        public void Generate()
        {
            IDynamoDbContext dynamoDbContext = FindAndCreateContext(_path, new DynamoDbSettings { UseLocalDatabase = true, EndpointUrl = _url});
            dynamoDbContext.CreateTableModel();

            var tableProperties = dynamoDbContext.GetType().GetProperties().Where(t => t.PropertyType.IsGenericType && t.PropertyType.GetGenericTypeDefinition() == typeof(DynamoDbTable<>));
            Console.WriteLine("------------------Aws Cli---------------------");

            var formatCommand = new FormatCommand { Indent = true, IndentLevel = 0 };
            StringBuilder rootBuilder = new StringBuilder();
            foreach (var prop in tableProperties)
            {
                formatCommand.ResetIndent();
                StringBuilder sb = new StringBuilder();

                var value = (IDynamoDbTableProperties)prop.GetValue(dynamoDbContext);
                ISerializer serializer = new CommandSerializer();

                if (_recreateTables)
                {
                    BaseCommand deleteBaseCommand = new BaseCommand();
                    deleteBaseCommand.Url = _url;

                    sb
                   .AppendLine("#------------------------------------------------")
                   .AppendLine($"# Delete table for {value.Name}")
                   .AppendLine("#------------------------------------------------")
                   .AppendLine();

                    deleteBaseCommand.DeleteTable = new DeleteTableCommand { TableName = value.Name };

                    sb.Append(serializer.Serialize(deleteBaseCommand, formatCommand)).AppendLine();

                }

                formatCommand.ResetIndent();
                
                BaseCommand createBaseCommand = new BaseCommand();
                createBaseCommand.Url = _url;
                

                sb
                    .AppendLine("#------------------------------------------------")
                    .AppendLine($"# Create table for {value.Name}")
                    .AppendLine("#------------------------------------------------")
                    .AppendLine();
                CreateTable createTable = new CreateTable() { TableName = value.Name };
                createBaseCommand.CreateTable = createTable;

                if (value.TableAttributes.Any()) createTable.AttributeDefinitions = new List<AttributeDefinitionOption>();
                foreach(var ad in value.TableAttributes)
                {
                    var ado = new AttributeDefinitionOption
                    {
                        AttributeName = ad.AttributeName,
                        AttributeType = ad.AttributeType
                    };
                    createTable.AttributeDefinitions.Add(ado);
                }

                if (value.TableKeySchema.Any()) createTable.KeySchema = new List<KeySchemaOption>();
                foreach(var ks in value.TableKeySchema)
                {
                    var kso = new KeySchemaOption
                    {
                        AttributeName = ks.AttributeName,
                        KeyType = ks.KeyType
                    };
                    createTable.KeySchema.Add(kso);
                }

                if(value.ProvisionedThroughput != null)
                {
                    createTable.ProvisinedThroughPut = new ProvisionedThroughputOption
                    {
                        ReadCapacityUnits = Convert.ToString(value.ProvisionedThroughput.ReadCapacityUnits),
                        WriteCapacityUnits = Convert.ToString(value.ProvisionedThroughput.WriteCapacityUnits)
                    };
                }

                if(value.GlobalSecondaryIndexList.Any())
                {
                    var jsonObject = new JsonObject<GlobalSecondaryIndexJsonModel>();
                    jsonObject.Value = new List<GlobalSecondaryIndexJsonModel>();
                    jsonObject.Value.AddRange(value.GlobalSecondaryIndexList.Select(t=>t.ConvertToJsonModel()));
                    

                    createTable.GlobalSecondaryIndexs = new GlobalSecondaryIndexOption
                    {
                        JsonObject = jsonObject
                    };
                }

                if(value.LocalSecondaryIndexList.Any())
                {
                    var jsonObject = new JsonObject<LocalSecondaryIndexJsonModel>();
                    jsonObject.Value = new List<LocalSecondaryIndexJsonModel>();
                    jsonObject.Value.AddRange(value.LocalSecondaryIndexList.Select(t=>t.ConvertToJsonModel()));

                    createTable.LocalSecondaryIndexs = new LocalSecondaryIndexOption
                    {
                        JsonObject = jsonObject
                    };
                }
                
                sb.Append(serializer.Serialize(createBaseCommand, formatCommand));
                sb.AppendLine();
                
                rootBuilder.Append(sb.ToString());
            }

            Console.WriteLine(rootBuilder.ToString());

            if(_scriptType == "ps")
            {
                var writer = new PowerShellScriptWriter();
                writer.Write(_outputPath, dynamoDbContext.GetType().Name, rootBuilder.ToString());
            }
        }
    }
}
