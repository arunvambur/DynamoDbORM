using DynamoDbORM.Domain.Exceptions;
using DynamoDbORM.Domain.SeedWork;

namespace DynamoDbORM.Domain.OrderAggregate
{
    public class OrderItem
    
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Units { get; set; }

        public int ProductId { get; private set; }

        protected OrderItem() { }

        public OrderItem(int productId, string productName, decimal unitPrice, decimal discount,  int units = 1)
        {
            if (units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            if ((unitPrice * units) < discount)
            {
                throw new OrderingDomainException("The total of order item is lower than applied discount");
            }

            ProductId = productId;

            ProductName = productName;
            UnitPrice = unitPrice;
            Discount = discount;
            Units = units;            
        }

         public decimal GetCurrentDiscount()
        {
            return Discount;
        }

        public int GetUnits()
        {
            return Units;
        }

        public decimal GetUnitPrice()
        {
            return UnitPrice;
        }

        public string GetOrderItemProductName() => ProductName;

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new OrderingDomainException("Discount is not valid");
            }

            Discount = discount;
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}