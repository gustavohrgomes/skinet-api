using API.Errors;
using API.Extensions;
using API.Models.Request;
using API.Models.Response;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : MainController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetOrders()
        {
            var email = User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<OrderResponse>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdForUser(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order is null) return NotFound(new ApiResponse((int)HttpStatusCode.NotFound, "Order Not Found"));

            return Ok(_mapper.Map<OrderResponse>(order));
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderRequest orderRequest)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<Address>(orderRequest.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderRequest.DeliveryMethodId, orderRequest.BasketId, address);

            if (order is null)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, "Problem creating Order"));
            }

            return Ok(order);
        }
    }
}