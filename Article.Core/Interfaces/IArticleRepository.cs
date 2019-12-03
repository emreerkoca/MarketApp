using Article.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Article.Core.Interfaces
{
    public interface IArticleRepository : IAsyncRepository<ArticleModel>
    {

    }
}
