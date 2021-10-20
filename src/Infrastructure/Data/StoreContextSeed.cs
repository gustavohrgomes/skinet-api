using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await SeedDatabase(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        private static async Task SeedDatabase(StoreContext context)
        {
            await SeedBrands(context);
            await SeedProductTypes(context);
            await SeedProducts(context);
            await SeedDeliveryMethods(context);
        }

        private static async Task SeedBrands(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                foreach (var brand in brands)
                {
                    context.ProductBrands.Add(brand);
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProductTypes(StoreContext context)
        {
            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                foreach (var type in types)
                {
                    context.ProductTypes.Add(type);
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProducts(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                foreach (var product in products)
                {
                    context.Products.Add(product);
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedDeliveryMethods(StoreContext context)
        {
            if (!context.DeliveryMethods.Any())
            {
                var data = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);

                foreach (var deliveryMethod in deliveryMethods)
                {
                    context.DeliveryMethods.Add(deliveryMethod);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
