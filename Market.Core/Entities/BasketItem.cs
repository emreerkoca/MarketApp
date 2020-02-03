using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Core.Entities
{
    public class BasketItem : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
