using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb
{
    public interface IDynamoDbTableProperties
    {
        string Name { get; }
        List<AttributeDefinition> TableAttributes { get; }
        List<KeySchemaElement> TableKeySchema { get; }
        ProvisionedThroughput ProvisionedThroughput { get; }
        List<LocalSecondaryIndex> LocalSecondaryIndexList { get; }
        List<GlobalSecondaryIndex> GlobalSecondaryIndexList { get; }
    }
}
