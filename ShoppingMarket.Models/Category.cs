using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShoppingMarket.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Name value cannot exceed 100 characters.")]
        public string Name { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Product>? Products { get; set; }
    }
}
