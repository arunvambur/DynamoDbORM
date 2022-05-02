using MediatR;
using DynamoDbORM.Domain.OrderAggregate;
using DynamoDbORM.Persistence.DynamoDb;
using System.Threading;
using System.Threading.Tasks;

namespace DynamoDbORM.Application.Queries
{
    public class OrderQueryHandler : IRequestHandler<OrderQuery, OrderQueryResponse>
    {
        private readonly ICommonRepository<Order> _commonRepository;

        public OrderQueryHandler(ICommonRepository<Order> commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public async Task<OrderQueryResponse> Handle(OrderQuery request, CancellationToken cancellationToken)
        {
            var configResult = await _commonRepository.GetItemByNameAsync(request.Name);

            var result = ConvertToOrderQueryResponse(configResult);
            return result;           
        }

        private OrderQueryResponse ConvertToOrderQueryResponse(Order result)
        {
            return new OrderQueryResponse() { Order = result };
        }
    }
}
