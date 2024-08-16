using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Data;

namespace ShoppingMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites(int customerId, int productId)
        {
            await _favoriteService.AddToFavoritesAsync(customerId, productId);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromFavorites(int customerId, int productId)
        {
            await _favoriteService.RemoveFromFavoritesAsync(customerId, productId);
            return Ok();
        }

        [HttpGet("products/{customerId}")]
        public async Task<IActionResult> GetFavoriteProducts(int customerId)
        {
            var products = await _favoriteService.GetFavoriteProductsAsync(customerId);
            return Ok(products);
        }
    }
}
