using Article.Core.Entities;
using Article.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Infrastructure.Data
{
    public class ArticleRepository : EfRepository<ArticleModel>, IArticleRepository
    {
        public ArticleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<IReadOnlyList<ArticleModel>> SearchArticlesByContent(string searchText)
        {
            var articles = await _appDbContext.Article
                .Where(x => x.Title.Contains(searchText) || x.Content.Contains(searchText))
                .ToListAsync();

            return articles;
        }

        public async Task<IReadOnlyList<ArticleModel>> SearchArticlesByKeywords(string[] keywords)
        {
            var articlesQueryable = _appDbContext.Article.AsQueryable();

            foreach (var keyword in keywords)
            {
                string currentKeyword = keyword;

                articlesQueryable = articlesQueryable.Where(x => x.Keywords.Contains(currentKeyword));
            }

            var articles = await articlesQueryable.ToListAsync();

            return articles;
        }
    }
}
