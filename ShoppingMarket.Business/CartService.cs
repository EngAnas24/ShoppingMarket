using AutoMapper;
using ShoppingMarket.Models.DTOS;
using ShoppingMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingMarket.Data;

namespace ShoppingMarket.Business
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public CartService(
            IRepository<Cart> cartRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Product> productRepository,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddToCartAsync(int customerId, int productId, int quantity)
        {
            var cart = (await _cartRepository.GetAllAsync()).FirstOrDefault(c => c.CustomerId == customerId);

            if (cart == null)
            {
                cart = new Cart { CustomerId = customerId };
                await _cartRepository.AddAsync(cart);
            }

            var cartItem = (await _cartItemRepository.GetAllAsync())
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                };
                await _cartItemRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                await _cartItemRepository.UpdateAsync(cartItem.Id, cartItem);
            }
        }

        public async Task RemoveFromCartAsync(int customerId, int productId)
        {
            var cart = (await _cartRepository.GetAllAsync()).FirstOrDefault(c => c.CustomerId == customerId);
            if (cart != null)
            {
                var cartItem = (await _cartItemRepository.GetAllAsync())
                    .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

                if (cartItem != null)
                {
                    await _cartItemRepository.DeleteAsync(cartItem.Id);
                }
            }
        }

        public async Task<CartDTO> GetCartItemsAsync(int customerId)
        {
            var cart = (await _cartRepository.GetAllAsync()).FirstOrDefault(c => c.CustomerId == customerId);
            return _mapper.Map<CartDTO>(cart);
        }
    }
}

