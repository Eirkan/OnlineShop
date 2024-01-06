using Order.Core.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entities.OrderAggregate
{
    public sealed class OrderItem : Entity<int>
    {
        [Required]
        private string _productName;
        private decimal _unitPrice;
        private decimal _discount;
        private int _units;

        public int ProductId { get; private set; }

        public OrderItem() : base(default) { }

        public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string PictureUrl, int units = 1) : this()
        {
            //if (units <= 0)
            //{
            //    throw new OrderingDomainException("Invalid number of units");
            //}

            //if ((unitPrice * units) < discount)
            //{
            //    throw new OrderingDomainException("The total of order item is lower than applied discount");
            //}

            ProductId = productId;

            _productName = productName;
            _unitPrice = unitPrice;
            _discount = discount;
            _units = units;
        }

        public decimal GetCurrentDiscount()
        {
            return _discount;
        }

        public int GetUnits()
        {
            return _units;
        }

        public decimal GetUnitPrice()
        {
            return _unitPrice;
        }

        public string GetOrderItemProductName() => _productName;

        public void SetNewDiscount(decimal discount)
        {
            //if (discount < 0)
            //{
            //    throw new OrderingDomainException("Discount is not valid");
            //}

            _discount = discount;
        }

        public void AddUnits(int units)
        {
            //if (units < 0)
            //{
            //    throw new OrderingDomainException("Invalid units");
            //}

            _units += units;
        }
    }
}
