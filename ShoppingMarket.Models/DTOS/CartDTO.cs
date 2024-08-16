using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMarket.Models.DTOS
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public IList<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
