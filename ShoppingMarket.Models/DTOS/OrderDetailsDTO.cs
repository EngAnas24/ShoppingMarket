using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace ShoppingMarket.Models.DTOS
{
    public class OrderDetailsDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public decimal? TotalPrice { get;set; }

        public void TotalPriceCalculator()
        {
            TotalPrice = UnitPrice * Quantity;
        }
    }

  
}
