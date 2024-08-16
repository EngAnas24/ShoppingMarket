using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;

namespace ShoppingMarket.Business
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> ProductRepository, IMapper mapper)
        {
            _productRepository = ProductRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            var Product = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(Product);
        }

        public async Task<ProductDTO> GetProductAsync(int id)
        {
            var catgry = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(catgry);

        }

        public async Task AddProductAsync(ProductDTO ProductDTO)
        {
            var Product = _mapper.Map<Product>(ProductDTO);
            await _productRepository.AddAsync(Product);
            ProductDTO.Id = Product.Id;
        }

        public async Task UpdateProductAsync(ProductDTO ProductDTO, int id)
        {
            var UpdatedProduct = _mapper.Map<Product>(ProductDTO);
            await _productRepository.UpdateAsync(id, UpdatedProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }
        public IEnumerable<ProductDTO> GetProducts(int id)
        {
            var QueryProduct = _productRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductDTO>>(QueryProduct);
        }
        public ProductDTO GetProductById(int id)
        {
            var QueryProduct = _productRepository.GetAllById(id);
            return _mapper.Map<ProductDTO>(QueryProduct);
        }

        public async Task<IEnumerable<ProductDTO>> SearchForProductsAsync(string SearchItem)
        {
            var SearchProduct = await _productRepository.SearchAsync(SearchItem);
            return _mapper.Map<IEnumerable<ProductDTO>>(SearchProduct);

        }


    }
}
