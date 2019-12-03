using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Core.Entities;
using Article.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Article.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        #region Fields
        IArticleRepository _articleRepository;
        #endregion

        #region Ctor
        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        #endregion

        #region Add Article
        [HttpPost("AddArticle")]
        public async Task<IActionResult> AddArticle(ArticleModel newArticle)
        {
            var result = await _articleRepository.AddAsync(newArticle);

            if (result == null)
            {
                return BadRequest("Could not add!");
            }

            return Ok(newArticle);
        }
        #endregion

        #region Get Articles
        [HttpGet("GetArticles")]
        public async Task<IActionResult> GetArticles()
        {
            IReadOnlyList<ArticleModel> articleList = await _articleRepository.GetListAllAsync();

            if (articleList == null)
            {
                return BadRequest("Could not get articles");
            }

            return Ok(articleList);
        }
        #endregion

        #region Update Article   
        [HttpPut("UpdateArticle")]
        public async Task<IActionResult> UpdateArticle(ArticleModel updatedArticle)
        {
            var result = await _articleRepository.UpdateAsync(updatedArticle);

            if (!result)
            {
                return BadRequest("Could not update article.");
            }

            return Ok(updatedArticle);
        }
        #endregion

        #region Delete Article
        [HttpDelete("DeleteArticle")]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            ArticleModel deletedArticle = await _articleRepository.GetByIdAsync(articleId);

            var result = await _articleRepository.DeleteAsync(deletedArticle);

            if (!result)
            {
                return BadRequest("Could not delete article");
            }

            return Ok();
        }
        #endregion

        #region Searc Articles By Content 
        [HttpPost("SearchArticlesByContent")]
        public async Task<IActionResult> SearchArticlesByContent([FromBody] string searchText)
        {
            IReadOnlyList<ArticleModel> articleList = await _articleRepository.SearchArticlesByContent(searchText);

            if (articleList == null)
            {
                return BadRequest("Could not get articles");
            }

            return Ok(articleList);
        }
        #endregion

        #region Searc Articles By Keywords 
        [HttpPost("SearchArticlesByKeywords")]
        public async Task<IActionResult> SearchArticlesByContent([FromBody] string[] keywords)
        {
            IReadOnlyList<ArticleModel> articleList = await _articleRepository.SearchArticlesByKeywords(keywords);

            if (articleList == null)
            {
                return BadRequest("Could not get articles");
            }

            return Ok(articleList);
        }
        #endregion
    }
}