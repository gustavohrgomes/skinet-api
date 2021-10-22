using API.Models.ViewModels;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace API.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemViewModel, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Resolve(OrderItem source, OrderItemViewModel destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return null;
            }

            return _configuration["ApiUrl"] + source.ItemOrdered.PictureUrl;
        }
    }
}