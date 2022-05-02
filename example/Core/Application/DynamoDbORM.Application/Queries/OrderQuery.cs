using MediatR;
using System;

namespace DynamoDbORM.Application.Queries
{
    public class OrderQuery : IRequest<OrderQueryResponse>
    {
        public String Name { get; set; }
        public string Id { get; set; }

    }
}
