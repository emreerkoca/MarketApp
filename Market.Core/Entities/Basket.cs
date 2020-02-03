using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Core.Entities
{
    public class Basket : BaseEntity
    {
        public int UserId { get; set; }
        public ICollection<BasketItem> Items { get; set; }
    }
}
