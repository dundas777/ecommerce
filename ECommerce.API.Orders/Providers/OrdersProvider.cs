using AutoMapper;
using ECommerce.API.Orders.Db;
using ECommerce.API.Orders.Interfaces;
using ECommerce.API.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("1/1/2020"),
                    Total = 123.45M,
                    Items = new List<Db.OrderItem>() { 
                        new Db.OrderItem() { Id = 1, ProductId = 1, OrderId = 1, UnitPrice = 100.00M, Quantity = 1 },
                        new Db.OrderItem() { Id = 2, ProductId = 2, OrderId = 1, UnitPrice = 22.00M, Quantity = 2 } 
                    }
                });
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 2, OrderDate = DateTime.Parse("2/2/2020"), Total = 22.22M,
                    Items = new List<Db.OrderItem>() {
                        new Db.OrderItem() { Id = 3, ProductId = 3, OrderId = 2, UnitPrice = 20.00M, Quantity = 2 },
                        new Db.OrderItem() { Id = 4, ProductId = 2, OrderId = 2, UnitPrice = 5.00M, Quantity = 3 }
                    }
                });
                dbContext.Orders.Add(new Db.Order() { Id = 3, CustomerId = 3, OrderDate = DateTime.Parse("3/3/2020"), Total = 345.33M,
                    Items = new List<Db.OrderItem>() {
                        new Db.OrderItem() { Id = 5, ProductId = 3, OrderId = 3, UnitPrice = 100.00M, Quantity = 4 }
                    }
                });
                dbContext.SaveChanges();

            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int CustomerId)
        {
            try
            {
                var orders = await dbContext.Orders.Include(o => o.Items).Where(o => o.CustomerId == CustomerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Order Order, string ErrorMessage)> GetOrderAsync(int id)
        {
            try
            {
                var order = await dbContext.Orders.FirstOrDefaultAsync(p => p.Id == id);
                if (order != null)
                {
                    var result = mapper.Map<Db.Order, Models.Order>(order);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
