using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models.DTOS;
using ShoppingMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMarket.Business
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public FavoriteService(IRepository<Favorite> favoriteRepository, IRepository<Product> productRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddToFavoritesAsync(int customerId, int productId)
        {
            var favorite = new Favorite { CustomerId = customerId, ProductId = productId };
            await _favoriteRepository.AddAsync(favorite);
        }

        public async Task RemoveFromFavoritesAsync(int customerId, int productId)
        {
            var favorite = (await _favoriteRepository.GetAllAsync())
                .FirstOrDefault(f => f.CustomerId == customerId && f.ProductId == productId);

            if (favorite != null)
            {
                await _favoriteRepository.DeleteAsync(favorite.Id);
            }
        }

        public async Task<IList<ProductDTO>> GetFavoriteProductsAsync(int customerId)
        {
            var products = (await _favoriteRepository.GetAllAsync())
            .Where(f => f.CustomerId == customerId)
                .Select(f => f.Product)
                .ToList();
            return _mapper.Map<IList<ProductDTO>>(products);
        }
    }
}
