using Article.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Article.Core.Interfaces
{
    public interface IArticleRepository : IAsyncRepository<ArticleModel>
    {
        Task<IReadOnlyList<ArticleModel>> SearchArticlesByContent(string searchText);
        Task<IReadOnlyList<ArticleModel>> SearchArticlesByKeywords(string[] keywords);
    }
}
