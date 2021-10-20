using API.Errors;
using API.Extensions;
using API.Models.Request;
using API.Models.Response;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
