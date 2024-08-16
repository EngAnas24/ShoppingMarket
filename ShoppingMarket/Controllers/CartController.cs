using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Data;

namespace ShoppingMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int customerId, int productId, int quantity)
        {
            await _cartService.AddToCartAsync(customerId, productId, quantity);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart(int customerId, int productId)
        {
            await _cartService.RemoveFromCartAsync(customerId, productId);
            return Ok();
        }

        [HttpGet("items/{customerId}")]
        public async Task<IActionResult> GetCartItems(int customerId)
        {
            var cartDto = await _cartService.GetCartItemsAsync(customerId);
            return Ok(cartDto);
        }
    }
}
