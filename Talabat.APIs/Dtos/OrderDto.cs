using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entity.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; }
    }
}
 