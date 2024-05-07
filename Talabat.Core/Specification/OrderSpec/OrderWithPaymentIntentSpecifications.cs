using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity.Order_Aggregate;

namespace Talabat.Core.Specification.OrderSpec
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string paymentIntentId) 
            :base(O => O.PaymentIntendId == paymentIntentId)
        {
            
        }
    }
}
