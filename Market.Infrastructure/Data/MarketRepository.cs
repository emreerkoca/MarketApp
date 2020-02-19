using Market.Core.Entities;
using Market.Core.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Data
{
    public class MarketRepository : EfRepository<Product>, IMarketRepository
    {
        public IConfiguration Configuration { get; }

        public MarketRepository(AppDbContext appDbContext, IConfiguration configuration) : base(appDbContext)
        {
            Configuration = configuration;
        }

        public async Task<IReadOnlyList<Product>> SearchProductsByCategory(string category)
        {
            var products = await _appDbContext.Product
                .Where(x => x.Name.Contains(category))
                .ToListAsync();

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
