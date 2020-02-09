using Market.Core.Entities;
using Market.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Data
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IReadOnlyList<Product>> SearchProductsByCategory(string category)
        {
            var products = await _appDbContext.Product
                .Where(x => x.Name.Contains(category))
                .ToListAsync(); //todo: change this. Category won' t be like this.

            return products;
        }

        public async Task<IReadOnlyList<Product>> SearchProductsByKeywords(string[] keywords)
        {
            var productsQueryable = _appDbContext.Product.AsQueryable();

            foreach (var keyword in keywords)
            {
                string currentKeyword = keyword;

                productsQueryable = productsQueryable.Where(x => x.Keywords.Contains(currentKeyword));
            }

            var products = await productsQueryable.ToListAsync();

            return products;
        }
    }
}
