using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Business;
using ShoppingMarket.Models.DTOS;
using System.Threading.Tasks;

namespace ShoppingMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _ProductService;

        public ProductController(ProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var Products = await _ProductService.GetProductsAsync();
            if (Products == null || !Products.Any())
            {
                return NotFound("There are no Products available.");
            }
            return Ok(Products);
        }

        [HttpGet("GetProduct/{id:int}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var Product = await _ProductService.GetProductAsync(id);
            if (Product == null)
            {
                return NotFound($"No Product found with ID: {id}");
            }
            return Ok(Product);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductAsync(ProductDTO ProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Product data.");
            }
            if (ProductDTO.CategoryId<=0)
            {
                return BadRequest("Failed to add Product.");
            }
            await _ProductService.AddProductAsync(ProductDTO);
            return Ok(ProductDTO);
        }

        [HttpPut("UpdateProduct/{Id:int}")]
        public async Task<IActionResult> UpdateProductAsync(int Id, ProductDTO ProductDTO)
        {
            if (Id <= 0 || Id != ProductDTO.Id)
            {
                return BadRequest("Invalid Product's Id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _ProductService.UpdateProductAsync(ProductDTO, Id);
            return Ok(ProductDTO);
        }
        [HttpDelete("DeleteProduct/{Id:int}")]
        public async Task<IActionResult> DeleteProductAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Id <= 0)
            {
                return BadRequest("Invalid Product's Id.");
            }
            await _ProductService.DeleteProductAsync(Id);
            return Ok("Deleted successfully");
        }

        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProductAsync(string searchItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(searchItem))
            {
                return BadRequest("Invalid Product.");
            }
            var result = await _ProductService.SearchForProductsAsync(searchItem);
            return Ok(result);
        }



    }
}
