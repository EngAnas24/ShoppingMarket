using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace ShoppingMarket.Models.DTOS
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }
        public IFormFile? ProductImageFile1 { get; set; }
        public IFormFile? ProductImageFile2 { get; set; }
        public IFormFile? ProductImageFile3 { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
