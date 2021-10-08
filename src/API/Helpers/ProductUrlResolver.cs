using API.Models.Response;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductResponse, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            var apiUrl = new StringBuilder();

            if (string.IsNullOrEmpty(source.PictureUrl)) return apiUrl.ToString();

            apiUrl.Append(_configuration.GetValue<string>("ApiUrl"));
            apiUrl.Append(source.PictureUrl);

            return apiUrl.ToString();
        }
    }
}