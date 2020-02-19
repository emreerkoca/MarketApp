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
        private readonly ISupplierRepository _supplierRepository;
        #endregion

        public SupplierController(ISupplierRepository productRepository)
        {
            _supplierRepository = productRepository;
        }

        #region Add Product
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] Product newProduct)
        {
            var result = await _supplierRepository.AddAsync(newProduct);

            if (result == null)
            {
                return BadRequest("Could not add!");
            }

            return Ok(newProduct);
        }
        #endregion

        #region Get All Products
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            IReadOnlyList<Product> productList = await _supplierRepository.GetListAllAsync();

            if (productList == null)
            {
                return BadRequest("Could not get products");
            }

            return Ok(productList);
        }
        #endregion

        #region Update Product   
        [HttpPut("update-product/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product updatedProduct)
        {
            var product = _supplierRepository.GetByIdAsync(productId).Result;


            if (product != null)
            {
                product.Name = updatedProduct.Name;

                var result = await _supplierRepository.UpdateAsync(product);

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
        [HttpDelete("delete-product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            Product deletedProduct = await _supplierRepository.GetByIdAsync(productId);

            var result = await _supplierRepository.DeleteAsync(deletedProduct);

            if (!result)
            {
                return BadRequest("Could not delete product");
            }

            return Ok();
        }
        #endregion
    }
}