using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<IReadOnlyList<Product>> SearchProductsByCategory(string searchText);
        Task<IReadOnlyList<Product>> SearchProductsByKeywords(string[] keywords);
    }
}
