using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Reflection;

namespace DynamoDb
{
    public abstract class DynamoDbContext : IDynamoDbContext
    {
        private AmazonDynamoDBClient _client;
        private readonly DynamoDbSettings _dynamoDbSettings;
        public AmazonDynamoDBClient Client { get { return _client; } }

        public DynamoDbContext()
        {

        }

        public DynamoDbContext(DynamoDbSettings dynamoDbSettings)
        {
            _dynamoDbSettings = dynamoDbSettings;
            if(_dynamoDbSettings is not null)
                CreateClient(_dynamoDbSettings.UseLocalDatabase);
        }

        public bool CreateClient(bool useDynamoDbLocal)
        {
            if (useDynamoDbLocal)
            {
                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                //var portUsed = IsPortInUse(_dynamoDbSettings.Port);
                //if (portUsed)
                //{
                //    return (false);
                //}

                // DynamoDB-Local is running, so create a client
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                //AWSCredentials credential = new AWSCredentials();
                ddbConfig.ServiceURL = _dynamoDbSettings.EndpointUrl;
                try
                {
                    _client = new AmazonDynamoDBClient(ddbConfig);
                }
                catch (Exception ex)
                {
                    throw new DynamoDbClientException("FAILED to create a DynamoDBLocal client", ex) { MessageCode = "PS0040002" };
                }
            }
            else
            {
                _client = new AmazonDynamoDBClient();
            }
            return true;
        }

        private static bool IsPortInUse(int port)
        {
            bool isAvailable = true;
            // Evaluate current system TCP connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        public Task CreateTableModel()
        {
            return OnCreateTableModel();
        }

        public async Task DeployTableModel()
        {
            var dynamoDbTables = this.GetType().GetProperties().Where(t=>t.PropertyType.IsGenericType);

            foreach(var dyTable in dynamoDbTables)
            {
                var isTableExist = await CallGetByReflection(dyTable.PropertyType, dyTable.GetValue(this), nameof(IDynamoDbTableOperation.IsTableExistAsync));
                if (!(bool)isTableExist)
                {
                    var isTableCreated = await CallGetByReflection(dyTable.PropertyType, dyTable.GetValue(this), nameof(IDynamoDbTableOperation.CreateTable));
                    //Todo: Handle error if table is not created
                }
            }
        }

        private static async Task<object> CallGetByReflection(Type dyTableRef, object refProp, string methodName)
        {
            var method = dyTableRef.GetMethod(methodName);
            var task = (Task)method.Invoke(refProp, null);
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }

        public async Task DeleteTableModel(string[] filtertables = null)
        {
            Type type = this.GetType();
            var properties = type.GetProperties().Where(t=>t.PropertyType.IsGenericType);

            List<string> tablesTobeDeleted = new List<string>();
            foreach(var p in properties)
            {
                if(p.PropertyType.GetGenericTypeDefinition() == typeof(DynamoDbTable<>))
                {
                    var tableType = p.PropertyType.GetGenericArguments().First();

                    //Todo: Get the table Name property from the type instead of type name
                    if (filtertables is not null)
                    {
                        if(filtertables.Contains(tableType.Name))
                        {
                            tablesTobeDeleted.Add(tableType.Name);
                        }
                    }
                    else
                    {
                        tablesTobeDeleted.Add(tableType.Name);
                    }
                }
            }

            foreach(var table in tablesTobeDeleted)
            {
                try
                {
                    //Todo: Run the deletion in parallel instead of sequential
                    await _client.DeleteTableAsync(table);
                }
                catch (ResourceNotFoundException)
                {
                    //Don't do any thing
                }
            }
        }

        public abstract Task OnCreateTableModel();        
    }

    public class DynamoDbContext<T> : DynamoDbContext 
    {
        public DynamoDbTable<T> CurrentTable { get; set; }

        public DynamoDbContext(DynamoDbSettings dynamoDbSettings) : base(dynamoDbSettings)
        {
            CurrentTable = new DynamoDbTable<T>(Client);
        }

        public override Task OnCreateTableModel()
        {
            return Task.CompletedTask;
        }
    }
}
