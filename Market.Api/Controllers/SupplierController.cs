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
    public class SupplierController : ControllerBase
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        #endregion

        public SupplierController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #region Add Product
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product newProduct)
        {
            var result = await _productRepository.AddAsync(newProduct);

            if (result == null)
            {
                return BadRequest("Could not add!");
            }

            return Ok(newProduct);
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

        #region Update Product   
        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product updatedProduct)
        {
            var product = _productRepository.GetByIdAsync(productId).Result;


            if (product != null)
            {
                product.Name = updatedProduct.Name;

                var result = await _productRepository.UpdateAsync(product);

                if (!result)
                {
                    return BadRequest("Could not update product.");
                }

                return Ok(updatedProduct);
            }

            return BadRequest("Could not find product.");
        }
        #endregion

        #region Delete Product
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            Product deletedProduct = await _productRepository.GetByIdAsync(productId);

            var result = await _productRepository.DeleteAsync(deletedProduct);

            if (!result)
            {
                return BadRequest("Could not delete product");
            }

            return Ok();
        }
        #endregion
    }
}