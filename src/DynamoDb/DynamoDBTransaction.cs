using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;


namespace DynamoDb
{
    public class DynamoDBTransaction<T, U> : IDynamoDbTransaction<T, U>
    {
        private readonly AmazonDynamoDBClient _client;

        public DynamoDBTransaction(AmazonDynamoDBClient client)
        {
            _client = client;
        }

        public async Task<string> CreateItemWithTransaction(T item, U uniqueItem, CancellationToken cancellationToken = default)
        {
            try
            {
                string tableJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Document doc = Document.FromJson(tableJson);
                var table = Table.LoadTable(_client, item.GetType().GetTableName());
                string uniqueTableJson = JsonConvert.SerializeObject(uniqueItem, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Document uniqueDoc = Document.FromJson(uniqueTableJson);
                var uniqueTable = Table.LoadTable(_client, uniqueItem.GetType().GetTableName());
                
                var attrb = table.ToAttributeMap(doc);
                var uniqueAttrib = uniqueTable.ToAttributeMap(uniqueDoc);

                Put putItem = new Put() { TableName = table.TableName, Item = attrb };
                Put putUniqueItem = new Put() { TableName = uniqueTable.TableName, Item = uniqueAttrib };
                putUniqueItem.ReturnValuesOnConditionCheckFailure = ReturnValuesOnConditionCheckFailure.ALL_OLD;
                putUniqueItem.ConditionExpression = "attribute_not_exists(" + uniqueTable.HashKeys[0].ToString() + ")";

                List<TransactWriteItem> lst = new List<TransactWriteItem>();
                lst.Add(new TransactWriteItem { Put = putItem });
                lst.Add(new TransactWriteItem { Put = putUniqueItem });

                TransactWriteItemsRequest insertTransaction = new TransactWriteItemsRequest { TransactItems = lst };
                await _client.TransactWriteItemsAsync(insertTransaction);
                return doc[table.HashKeys[0]].ToString();
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException($"An error occured while inserting items into table", ex) { MessageCode = "PS0040004" };
            }
        }

        public async Task<bool> DeleteItemWithTransaction(T item, U uniqueItem, CancellationToken cancellationToken = default)
        {
            try
            {
                string tableJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Document doc = Document.FromJson(tableJson);
                var table = Table.LoadTable(_client, item.GetType().GetTableName());
                string uniqueTableJson = JsonConvert.SerializeObject(uniqueItem, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Document uniqueDoc = Document.FromJson(uniqueTableJson);
                var uniqueTable = Table.LoadTable(_client, uniqueItem.GetType().GetTableName());

                var attrb = table.ToAttributeMap(doc);
                var uniqueAttrib = uniqueTable.ToAttributeMap(uniqueDoc);

                Delete deleteItem = new Delete() { TableName = table.TableName };
                deleteItem.Key.Add(table.HashKeys[0].ToString(), attrb[table.HashKeys[0].ToString()]);
                deleteItem.Key.Add(table.RangeKeys[0].ToString(), attrb[table.RangeKeys[0].ToString()]);
                deleteItem.ConditionExpression = "attribute_exists(" + table.HashKeys[0].ToString() + ")";
                Delete deleteUniqueItem = new Delete() { TableName = uniqueTable.TableName };
                deleteUniqueItem.Key.Add(uniqueTable.HashKeys[0].ToString(), uniqueAttrib[uniqueTable.HashKeys[0].ToString()]);
                deleteUniqueItem.ConditionExpression = "attribute_exists(" + uniqueTable.HashKeys[0].ToString() + ")";

                List<TransactWriteItem> lst = new List<TransactWriteItem>();
                lst.Add(new TransactWriteItem { Delete = deleteItem });
                lst.Add(new TransactWriteItem { Delete = deleteUniqueItem });

                TransactWriteItemsRequest insertTransaction = new TransactWriteItemsRequest { TransactItems = lst };
                var m = await _client.TransactWriteItemsAsync(insertTransaction);

            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException($"Record not found in table") { MessageCode = "PS0040001" };
            }
            return true;
        }
    }
}
