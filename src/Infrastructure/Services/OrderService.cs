using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<DeliveryMethod> _deliveryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IRepository<Order> orderRepository, IRepository<DeliveryMethod> deliveryRepository, IRepository<Product> productRepository, IBasketRepository basketRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address address)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _productRepository.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _deliveryRepository.GetByIdAsync(deliveryMethodId);
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, buyerEmail, address, deliveryMethod, subtotal);

            // TODO: save to db

            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}