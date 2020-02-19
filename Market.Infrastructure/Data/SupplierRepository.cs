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
    public class SupplierRepository : EfRepository<Product>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
