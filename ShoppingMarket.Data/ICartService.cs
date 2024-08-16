using ShoppingMarket.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMarket.Data
{
    public interface ICartService
    {
        Task AddToCartAsync(int customerId, int productId, int quantity);
        Task RemoveFromCartAsync(int customerId, int productId);
        Task<CartDTO> GetCartItemsAsync(int customerId);
    }
}
