using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using DynamoDbORM.Domain.OrderAggregate;
using DynamoDb;


namespace DynamoDbORM.Persistence.DynamoDb
{
    public class OrderDbContext : DynamoDbContext
    {
        public DynamoDbTable<Order> OrderTable { get; set; }
     

        public OrderDbContext(DynamoDbSettings dynamoDbSettings) : base(dynamoDbSettings)
        {
            this.CreateClient(dynamoDbSettings.UseLocalDatabase);
           
            OrderTable = new DynamoDbTable<Order>(this.Client);
           
        }

        public override async Task OnCreateTableModel()
        {                       
           
            OrderTable.AddAttribute(a => a.Id, ScalarAttributeType.S)
                .AddAttribute(a => a.BuyerId, ScalarAttributeType.S)
                .AddKeySchema(a => a.Id, KeyType.HASH)
                .AddKeySchema(a => a.BuyerId, KeyType.RANGE)
                .AddGlobalSecondaryIndex("BuyerIdIndex", a => a.BuyerId, a => a.Id)
                .AddProvisionedThroughPut(1, 1);

            if (!await OrderTable.IsTableExistAsync())
            {
                await OrderTable.CreateTable();
            }
                       
          
        }
    }
}
