using DynamoDbORM.Domain.SeedWork;
using System.Collections.Generic;
using DynamoDb;

namespace DynamoDbORM.Domain.OrderAggregate
{
    [DynamoDBTable("gtp-ps-ddb-OrderTable")]
    public class Order
    : EntityWithId
    {
        public string BuyerId { get; set; }
        public string BuyerName { get; set; }
        public Address Address { get;  set; }
        public OrderStatus OrderStatus { get;  set; }
        public List<OrderItem> OrderItems { get; set; }
        public Order()
        {

        }
        public Order(string buyerId, string name, Address address)
        {
            this.BuyerId = buyerId; // Convert.ToInt32(buyerId);
            this.BuyerName = name;
            this.Address = address;
            OrderItems = new List<OrderItem>();


        }
        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, int units = 1)
        {
            //add validated new order item

            var orderItem = new OrderItem(productId, productName, unitPrice, discount, units);
            OrderItems.Add(orderItem);
        }
    }
}