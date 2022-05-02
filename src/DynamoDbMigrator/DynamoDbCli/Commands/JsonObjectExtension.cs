using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using DynamoDbMigrator.DynamoDbCli.Commands.JsonModel;

namespace DynamoDbMigrator.DynamoDbCli.Commands
{
    static class JsonObjectExtension
    {
        public static GlobalSecondaryIndexJsonModel ConvertToJsonModel(this GlobalSecondaryIndex globalSecondaryIndex )
        {
            return new GlobalSecondaryIndexJsonModel
            {
                IndexName = globalSecondaryIndex.IndexName,
                KeySchema = globalSecondaryIndex.KeySchema?.Select(t => t.ConvertToJsonModel()).ToList(),
                Projection = globalSecondaryIndex.Projection?.ConvertToJsonModel(),
                ProvisionedThroughput = globalSecondaryIndex.ProvisionedThroughput.ConvertToJsonModel()
            };
        }

        public static LocalSecondaryIndexJsonModel ConvertToJsonModel(this LocalSecondaryIndex localSecondaryIndex)
        {
            return new LocalSecondaryIndexJsonModel
            {
                IndexName = localSecondaryIndex.IndexName,
                KeySchema = localSecondaryIndex.KeySchema?.Select(t => t.ConvertToJsonModel()).ToList(),
                Projection = localSecondaryIndex.Projection?.ConvertToJsonModel()
            };
        }

        public static KeySchemaElementJsonModel ConvertToJsonModel(this KeySchemaElement keySchemaElement)
        {
            return new KeySchemaElementJsonModel
            {
                AttributeName = keySchemaElement.AttributeName,
                KeyType = keySchemaElement.KeyType.Value
            };
        }

        public static ProjectionJsonModel ConvertToJsonModel(this Projection projection)
        {
            return new ProjectionJsonModel
            {
                NonKeyAttributes = projection.NonKeyAttributes.Any() ? projection.NonKeyAttributes : null,
                ProjectionType = projection.ProjectionType.Value
            };
        }

        public static ProvisionedThroughputJsonModel ConvertToJsonModel(this ProvisionedThroughput provisionedThroughput)
        {
            return new ProvisionedThroughputJsonModel
            {
                ReadCapacityUnits = provisionedThroughput.ReadCapacityUnits,
                WriteCapacityUnits = provisionedThroughput.WriteCapacityUnits
            };
        }
    }
}
