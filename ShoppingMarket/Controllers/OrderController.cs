using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Business;
using ShoppingMarket.Models.DTOS;
using System.Threading.Tasks;

namespace ShoppingMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _OrderService;

        public OrderController(OrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var Orders = await _OrderService.GetOrdersAsync();
            if (Orders == null || !Orders.Any())
            {
                return NotFound("There are no Orders available.");
            }
            return Ok(Orders);
        }

        [HttpGet("GetOrder/{id:int}")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var Order = await _OrderService.GetOrderAsync(id);
            if (Order == null)
            {
                return NotFound($"No Order found with ID: {id}");
            }
            return Ok(Order);
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrderAsync(OrderDTO OrderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Order data.");
            }

            await _OrderService.AddOrderAsync(OrderDTO);
            return Ok(OrderDTO);
        }

        [HttpPut("UpdateOrder/{Id:int}")]
        public async Task<IActionResult> UpdateOrderAsync(int Id,OrderDTO OrderDTO)
        {
            if (Id <= 0||Id!=OrderDTO.Id) {
                return BadRequest("Invalid Order's Id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _OrderService.UpdateOrderAsync(OrderDTO,Id);
            return Ok(OrderDTO);
        }
        [HttpDelete("DeleteOrder/{Id:int}")]
        public async Task<IActionResult> DeleteOrderAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Id <= 0)
            {
                return BadRequest("Invalid Order's Id.");
            }
            await _OrderService.DeleteOrderAsync(Id);
            return Ok("Deleted successfully");
        }

        [HttpGet("SearchOrder")]
        public async Task<IActionResult> SearchOrderAsync(string searchItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(searchItem))
            {
                return BadRequest("Invalid Order.");
            }
            var result=await _OrderService.SearchForOrdersAsync(searchItem);
            return Ok(result);
        }



    }
}
