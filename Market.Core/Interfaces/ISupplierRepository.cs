using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Interfaces
{
    public interface ISupplierRepository : IAsyncRepository<Product>
    {
    }
}
