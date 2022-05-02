using MediatR;
using DynamoDbORM.Application.Commands.Create;
using DynamoDbORM.Domain.OrderAggregate;
using DynamoDbORM.Persistence.DynamoDb;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDbORM.Application
{
    // Regular CommandHandler
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly ICommonRepository<Order> _orderRepository;
        
        // Using DI to inject infrastructure persistence Repositories
        public CreateOrderCommandHandler(ICommonRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
        {
            var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
            var order = new Order(message.UserId, message.UserName, address);

            foreach (var item in message.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.Units);
            }
            order.OrderStatus = OrderStatus.Submitted;  

          await  _orderRepository.AddItemAsync(order);
            return new CreateOrderResponse { Message = "Order saved successfully." };

        }
    


    }
}