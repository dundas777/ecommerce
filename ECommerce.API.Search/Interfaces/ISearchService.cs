using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ECommerce.API.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(Boolean IsSuccess, dynamic SearchResults)> SearchAsync(int customerId);
    }
}
