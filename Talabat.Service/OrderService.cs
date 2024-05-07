using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entity;
using Talabat.Core.Entity.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
             IBasketRepository basketRepo,
             IUnitOfWork unitOfWork,
             IPaymentService paymentService
             ///IGenericRepository<Product> productRepo,
             ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
             ///IGenericRepository<Order> orderRepo
             )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
           _paymentService = paymentService;
            ///_productRepo = productRepo;
            ///_deliveryMethodRepo = deliveryMethodRepo;
            ///_orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1- Get basket form basket repo

            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2- Get selected items at basket from product repo

            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                var productRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.Id);

                    var productItemOrder = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3- Calculate subTotal

            var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            // 4- Get Delivery method from delivery method repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();

            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetEntityWithSpecAsync(orderSpecs);

            if(existingOrder != null)
            {
                 orderRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // 5- create order

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            await orderRepo.AddAsync(order);

            // 6- save to database

            var result = await _unitOfWork.CompleteAsync();
            if(result <= 0) return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var orderspec = new OrderSpecifications(orderId, buyerEmail);

            var order = orderRepo.GetEntityWithSpecAsync(orderspec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo   =_unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}
