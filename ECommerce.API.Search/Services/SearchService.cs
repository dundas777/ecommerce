using ECommerce.API.Search.Interfaces;
using ECommerce.API.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ECommerce.API.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService orderService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService orderService, IProductsService productsService, ICustomersService customersService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customersService = customersService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await orderService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            var customerResult = await customersService.GetCustomersAsync();

            if (ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product info is not available";
                    }
                    order.CustomerName = customerResult.IsSuccess ? 
                        customerResult.Customers.FirstOrDefault(c => c.Id == order.CustomerId)?.Name :
                        "Customer info is not available";
                }

                var result = new
                {
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
