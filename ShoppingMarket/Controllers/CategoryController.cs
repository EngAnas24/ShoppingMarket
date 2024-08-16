using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Business;
using ShoppingMarket.Models.DTOS;
using System.Threading.Tasks;

namespace ShoppingMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound("There are no categories available.");
            }
            return Ok(categories);
        }

        [HttpGet("GetByCategoryId/{id:int}")]
        public async Task<IActionResult> GetByCategoryIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound($"No category found with ID: {id}");
            }
            return Ok(category);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid category data.");
            }

            await _categoryService.AddCategoryAsync(categoryDTO);
            return Ok(categoryDTO);
        }

        [HttpPut("UpdateCategory/{Id:int}")]
        public async Task<IActionResult> UpdateCategoryAsync(int Id,CategoryDTO categoryDTO)
        {
            if (Id <= 0||Id!=categoryDTO.Id) {
                return BadRequest("Invalid category's Id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _categoryService.UpdateCategoryAsync(categoryDTO,Id);
            return Ok(categoryDTO);
        }
        [HttpDelete("DeleteCategory/{Id:int}")]
        public async Task<IActionResult> DeleteCategoryAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Id <= 0)
            {
                return BadRequest("Invalid category's Id.");
            }
            await _categoryService.DeleteCategoryAsync(Id);
            return Ok("Deleted successfully");
        }

        [HttpGet("SearchCategory")]
        public async Task<IActionResult> SearchCategoryAsync(string searchItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(searchItem))
            {
                return BadRequest("Invalid category.");
            }
            var result=await _categoryService.SearchForCategoriesAsync(searchItem);
            return Ok(result);
        }



    }
}
