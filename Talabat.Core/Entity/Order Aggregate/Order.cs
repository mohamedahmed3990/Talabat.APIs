using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entity.Order_Aggregate
{
    public class Order : BaseEntity
    {   
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntendId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } // Navigational prop 1

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // navigational prop M

        public decimal SubTotal { get; set; }


        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntendId { get; set; }

    }

}
