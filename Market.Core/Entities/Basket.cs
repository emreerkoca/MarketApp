using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Core.Entities
{
    public class Basket : BaseEntity
    {
        public ICollection<BasketItem> Items { get; set; }
        public bool ActivationStatus { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
