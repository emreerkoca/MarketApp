using Article.Core.Entities;
using Article.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Infrastructure.Data
{
    public class ArticleRepository : EfRepository<ArticleModel>, IArticleRepository
    {
        public ArticleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
