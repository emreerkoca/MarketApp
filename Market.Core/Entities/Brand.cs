using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Core.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
