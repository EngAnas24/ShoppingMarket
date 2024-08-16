using ShoppingMarket.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMarket.Data
{
    public interface IFavoriteService
    {
        Task AddToFavoritesAsync(int customerId, int productId);
        Task RemoveFromFavoritesAsync(int customerId, int productId);
        Task<IList<ProductDTO>> GetFavoriteProductsAsync(int customerId);
    }
}
