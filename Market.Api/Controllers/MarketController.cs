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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        #endregion

        #region Ctor
        public MarketController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region Get All Products
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            IReadOnlyList<Product> productList = await _productRepository.GetListAllAsync();

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion

        #region Get Products By Category 
        [HttpGet("SearchProductsByCategory/{categories}")]
        public async Task<IActionResult> SearchProductsByCategory(string categories)
        {
            IReadOnlyList<Product> productList = await _productRepository.SearchProductsByCategory(categories);

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion

        #region Search Products By Keywords 
        [HttpPost("SearchProductsByKeywords")]
        public async Task<IActionResult> SearchProductsByKeywords([FromBody] string[] keywords)
        {
            IReadOnlyList<Product> productList = await _productRepository.SearchProductsByKeywords(keywords);

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion
    }
}