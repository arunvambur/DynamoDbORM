using MediatR;
using DynamoDbORM.Domain.OrderAggregate;
using System.Collections.Generic;


namespace DynamoDbORM.Application.Commands.Create
{

    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public string UserId { get;  set; }
        public string UserName { get;  set; }
        public string City { get;  set; }
        public string Street { get;  set; }
        public string State { get;  set; }
        public string Country { get;  set; }
        public string ZipCode { get;  set; }
        public string CardNumber { get;  set; }
        public string CardHolderName { get;  set; }
        public OrderStatus OrderStatus { get; set; }     
        public List<OrderItem> OrderItems { get; set; }
        public CreateOrderCommand()
        {
            OrderItems = new List<OrderItem>();
        }

        public record OrderItem
        {
            public int ProductId { get; init; }
            public string ProductName { get; init; }
            public decimal UnitPrice { get; init; }
            public decimal Discount { get; init; }
            public int Units { get; init; }

        }
    }

}

