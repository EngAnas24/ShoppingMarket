using Microsoft.AspNetCore.Mvc;
using ShoppingMarket.Business;
using ShoppingMarket.Models.DTOS;
using System.Threading.Tasks;

namespace ShoppingMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _CustomerService;

        public CustomerController(CustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var Customers = await _CustomerService.GetCustomersAsync();
            if (Customers == null || !Customers.Any())
            {
                return NotFound("There are no Customers available.");
            }
            return Ok(Customers);
        }

        [HttpGet("GetCustomer/{id:int}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var Customer = await _CustomerService.GetCustomerAsync(id);
            if (Customer == null)
            {
                return NotFound($"No Customer found with ID: {id}");
            }
            return Ok(Customer);
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomerAsync(CustomerDTO CustomerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Customer data.");
            }

            await _CustomerService.AddCustomerAsync(CustomerDTO);
            return Ok(CustomerDTO);
        }

        [HttpPut("UpdateCustomer/{Id:int}")]
        public async Task<IActionResult> UpdateCustomerAsync(int Id,CustomerDTO CustomerDTO)
        {
            if (Id <= 0||Id!=CustomerDTO.Id) {
                return BadRequest("Invalid Customer's Id.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _CustomerService.UpdateCustomerAsync(CustomerDTO,Id);
            return Ok(CustomerDTO);
        }
        [HttpDelete("DeleteCustomer/{Id:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Id <= 0)
            {
                return BadRequest("Invalid Customer's Id.");
            }
            await _CustomerService.DeleteCustomerAsync(Id);
            return Ok("Deleted successfully");
        }

        [HttpGet("SearchCustomer")]
        public async Task<IActionResult> SearchCustomerAsync(string searchItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(searchItem))
            {
                return BadRequest("Invalid Customer.");
            }
            var result=await _CustomerService.SearchForCustomersAsync(searchItem);
            return Ok(result);
        }



    }
}
