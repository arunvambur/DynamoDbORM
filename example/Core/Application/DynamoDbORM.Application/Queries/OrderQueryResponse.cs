using DynamoDbORM.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbORM.Application.Queries
{
   public class OrderQueryResponse
    {
       public Order Order { get; set; }
    }
}
