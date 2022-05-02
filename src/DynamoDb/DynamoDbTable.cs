using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;

namespace DynamoDb
{
    public class DynamoDbTable<TTable> : IDynamoDbTableTransaction<TTable>, IDynamoDbTableOperation, IDynamoDbTableProperties
    {
        private readonly string _name;
        private readonly AmazonDynamoDBClient _client;

        private readonly List<AttributeDefinition> _tableAttributes;
        private readonly List<KeySchemaElement> _tableKeySchema;
        private ProvisionedThroughput _provisionedThroughput;
        private readonly List<LocalSecondaryIndex> _localSecondaryIndexList;
        private readonly List<GlobalSecondaryIndex> _globalSecondaryIndexList;

        public string Name => _name;

        public List<AttributeDefinition> TableAttributes => _tableAttributes;

        public List<KeySchemaElement> TableKeySchema => _tableKeySchema;

        public ProvisionedThroughput ProvisionedThroughput => _provisionedThroughput;

        public List<LocalSecondaryIndex> LocalSecondaryIndexList => _localSecondaryIndexList;

        public List<GlobalSecondaryIndex> GlobalSecondaryIndexList => _globalSecondaryIndexList;

        public DynamoDbTable(AmazonDynamoDBClient client)
        {
            _client = client;       
            _name = (typeof(TTable)).GetTableName();
            _tableAttributes = new List<AttributeDefinition>();
            _tableKeySchema = new List<KeySchemaElement>();
            _localSecondaryIndexList = new List<LocalSecondaryIndex>();
            _globalSecondaryIndexList = new List<GlobalSecondaryIndex>();
        }
     
        public DynamoDbTable<TTable> AddAttribute(Expression<Func<TTable, object>> expression, ScalarAttributeType attributeType)
        {
            _tableAttributes.Add(new AttributeDefinition(expression.GetMemberAccessList().Select(t => t.Name).First(), attributeType));
            return this;
        }

        public DynamoDbTable<TTable> AddKeySchema(Expression<Func<TTable, object>> expression, KeyType keyType)
        {
            _tableKeySchema.Add(new KeySchemaElement(expression.GetMemberAccessList().Select(t => t.Name).First(), keyType));
            return this;
        }

        public DynamoDbTable<TTable> AddProvisionedThroughPut(long readCapacityUnits, long writeCapacityUnits)
        {
            _provisionedThroughput = new ProvisionedThroughput(readCapacityUnits, writeCapacityUnits);
            return this;
        }
        public DynamoDbTable<TTable> AddLocalSecondaryIndex(string indexName, Expression<Func<TTable, object>> hashExpression, Expression<Func<TTable, object>> rangeExpression)
        {
            List<KeySchemaElement> schemaLst = new List<KeySchemaElement>();
            schemaLst.Add(new KeySchemaElement(hashExpression.GetMemberAccessList().Select(t => t.Name).First(), KeyType.HASH));
            schemaLst.Add(new KeySchemaElement(rangeExpression.GetMemberAccessList().Select(t => t.Name).First(), KeyType.RANGE));
            Projection projection = new Projection() { ProjectionType = "KEYS_ONLY" };
            _localSecondaryIndexList.Add(new LocalSecondaryIndex { IndexName = indexName, KeySchema = schemaLst, Projection = projection });
            return this;
        }
        public DynamoDbTable<TTable> AddGlobalSecondaryIndex(string indexName, Expression<Func<TTable, object>> hashExpression, Expression<Func<TTable, object>> rangeExpression)
        {
            List<KeySchemaElement> schemaLst = new List<KeySchemaElement>();
            schemaLst.Add(new KeySchemaElement(hashExpression.GetMemberAccessList().Select(t => t.Name).First(), KeyType.HASH));
            schemaLst.Add(new KeySchemaElement(rangeExpression.GetMemberAccessList().Select(t => t.Name).First(), KeyType.RANGE));
            Projection projection = new Projection() { ProjectionType = "ALL" };
            var provisionedThroughput = new ProvisionedThroughput(10, 1);
            _globalSecondaryIndexList.Add(new GlobalSecondaryIndex { IndexName = indexName, KeySchema = schemaLst, Projection = projection, ProvisionedThroughput = provisionedThroughput });
            return this;
        }
        
        public async Task<bool> CreateTable()
        {
            var request = new CreateTableRequest
            {
                TableName = _name,
                AttributeDefinitions = _tableAttributes,
                KeySchema = _tableKeySchema,
                ProvisionedThroughput = _provisionedThroughput,
                LocalSecondaryIndexes = _localSecondaryIndexList,
                GlobalSecondaryIndexes = _globalSecondaryIndexList
            };

            try
            {
                await _client.CreateTableAsync(request);
                return true;
            }
            catch (InternalServerErrorException ise)
            {
                throw new DynamoDbClientException(ise.Message, _name, ise);
            }
            catch (LimitExceededException lee)
            {
                throw new DynamoDbClientException(lee.Message, _name, lee);
            }
            catch (ResourceInUseException rie)
            {
                throw new DynamoDbClientException(rie.Message, _name, rie);
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException(ex.Message, _name, ex);
            }
        }

        public async Task<bool> DeleteTable()
        {
            var request = new DeleteTableRequest
            {
                TableName = _name
            };

            try
            {
                await _client.DeleteTableAsync(request);
                return true;
            }
            catch (InternalServerErrorException ise)
            {
                throw new DynamoDbClientException(ise.Message, _name, ise);
            }
            catch (LimitExceededException lee)
            {
                throw new DynamoDbClientException(lee.Message, _name, lee);
            }
            catch (ResourceInUseException rie)
            {
                throw new DynamoDbClientException(rie.Message, _name, rie);
            }
            catch (ResourceNotFoundException rne)
            {
                throw new DynamoDbClientException(rne.Message, _name, rne);
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException(ex.Message, _name, ex);
            }
        }

        public async Task<TableDescription> GetTableDescription()
        {
            TableDescription result = null;

            try
            {
                var response = await _client.DescribeTableAsync(_name);
                result = response.Table;
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException(ex.Message, _name, ex);
            }
            return result;
        }

        public async Task<bool> IsTableExistAsync()
        {
            var response = await _client.ListTablesAsync();
            return response.TableNames.Contains(_name);
        }

        public async Task<bool> CheckingTableExistenceAsync()
        {
            var response = await _client.ListTablesAsync();
            return response.TableNames.Contains(_name);
        }

        public async Task<string> CreateItem(TTable item, CancellationToken cancellationToken = default)
        {
            try
            {
                string tableJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Document doc = Document.FromJson(tableJson);
                var table = Table.LoadTable(_client, _name);

                await table.PutItemAsync(doc);

                return doc[table.HashKeys[0]].ToString();
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException($"An error occured while creating item in table {_name}", ex);
            }
        }

        public async Task UpdateItem(string hash, string range, TTable item, CancellationToken cancellationToken = default)
        {
            try
            {
                string tableJson = JsonConvert.SerializeObject(item, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                Document doc = Document.FromJson(tableJson);
                var table = Table.LoadTable(_client, _name);
                Primitive pHash = new Primitive(hash);
                Primitive pRange = new Primitive(range, false);

                await table.UpdateItemAsync(doc, pHash, pRange);
            }
            catch (Exception ex)
            {
                throw new DynamoDbClientException(hash, range, _name, $"An error occured while updating item from table {_name}", ex) { MessageCode = "PS0040003" };
            }
        }

        public async Task DeleteItem(string hash, string range, CancellationToken cancellationToken = default)
        {
            Primitive pHash = new Primitive(hash);
            Primitive pRange = new Primitive(range, false);
            var table = Table.LoadTable(_client, _name);

            var item = await table.GetItemAsync(pHash, pRange);
            if (item != null)
            {
                string tableJson = item.ToJson();
                Document doc = Document.FromJson(tableJson);
                await table.DeleteItemAsync(doc, cancellationToken);
            }
            else
            {
                throw new DynamoDbClientException(hash, range, _name, $"Record not found in table - {_name} for Hash - {hash} & Range - {range}") { MessageCode = "PS0040001" };
            }

        }

        public async Task<TTable> RetriveItem(string hash, string range)
        {

            Primitive pHash = new Primitive(hash);
            Primitive pRange = new Primitive(range, false);
            var table = Table.LoadTable(_client, _name);

            var item = await table.GetItemAsync(pHash, pRange);
            if (item != null)
            {
                return JsonConvert.DeserializeObject<TTable>(item.ToJson());
            }
            else
            {
                throw new DynamoDbClientException(hash, range, _name, $"Record not found in table - {_name} for Hash - {hash} & Range - {range}") { MessageCode = "PS0040001" };
            }
        }

        public async Task<TTable> RetriveItemByName(string name, bool isHash = false)
        {
            QueryFilter filter = new QueryFilter();
            var table = Table.LoadTable(_client, _name);
            Search search;
            if (isHash)
            {
                var pHash = new Primitive(name);
                search = table.Query(pHash, filter);
            }
            else
            {
                List<AttributeValue> valList = new List<AttributeValue>();
                AttributeValue val = new AttributeValue { S = name };
                valList.Add(val);
                filter = new QueryFilter(table.RangeKeys[0].ToString(), QueryOperator.Equal, valList);

                var conf = new QueryOperationConfig()
                {
                    IndexName = table.GlobalSecondaryIndexNames[0],
                    Filter = filter
                };
                search = table.Query(conf);
            }
            var documentSet = await search.GetNextSetAsync();
            var item = documentSet.FirstOrDefault();
            if (item != null)
            {
                return JsonConvert.DeserializeObject<TTable>(item.ToJson());
            }
            else
            {
                throw new DynamoDbClientException($"Record not found in table for Hash / Range - {name}") { MessageCode = "PS0040001" };
            }
        }
        public async Task<bool> CheckItemExists(string hash, string range)
        {
            try
            {
                var item = await RetriveItem(hash, range);
                if (object.Equals(item, default(TTable)))
                {
                    return true;
                }
            }
            // Indicates item not exists
            catch (Exception)
            {
            }
            return false;
        }
    }
}
