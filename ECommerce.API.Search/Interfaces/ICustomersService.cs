using ECommerce.API.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Search.Interfaces
{
    public interface ICustomersService
    {
        Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessaqge)> GetCustomersAsync();
    }
}
