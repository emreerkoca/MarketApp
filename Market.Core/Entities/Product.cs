using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Keywords { get; set; }
        public int StockCount { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

        public int? SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
