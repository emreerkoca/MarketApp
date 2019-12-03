using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Core.Entities
{
    public class ArticleModel : BaseEntity
    {
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public int ApplauseAmount { get; set; }
        public string Keywords { get; set; }
        public int OwnerId { get; set; }
    }
}
