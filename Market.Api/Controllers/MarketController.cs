using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Core.Entities;
using Market.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        #region Fields
        private readonly IMarketRepository _marketRepository;
        #endregion

        #region Ctor
        public MarketController(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }
        #endregion

        #region Get All Products
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            IReadOnlyList<Product> productList = await _marketRepository.GetListAllAsync();

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion

        #region Get Products By Category 
        [HttpGet("search/{categories}")]
        public async Task<IActionResult> SearchProductsByCategory(string categories)
        {
            IReadOnlyList<Product> productList = await _marketRepository.SearchProductsByCategory(categories);

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion

        #region Search Products By Keywords 
        [HttpPost("search-by-keywords")]
        public async Task<IActionResult> SearchProductsByKeywords([FromBody] string[] keywords)
        {
            IReadOnlyList<Product> productList = await _marketRepository.SearchProductsByKeywords(keywords);

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion
    }
}