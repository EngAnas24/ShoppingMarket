using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;

namespace ShoppingMarket.Business
{
    public class OrderDetailsService
    {
        private readonly IRepository<OrderDetails> _orderDetailsRepository;
        private readonly IMapper _mapper;

        public OrderDetailsService(IRepository<OrderDetails> OrderDetailsRepository, IMapper mapper)
        {
            _orderDetailsRepository = OrderDetailsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailsDTO>> GetCategoriesAsync()
        {
            var OrderDetails = await _orderDetailsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDetailsDTO>>(OrderDetails);
        }

        public async Task<OrderDetailsDTO> GetOrderDetailsAsync(int id)
        {
            var catgry = await _orderDetailsRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDetailsDTO>(catgry);

        }

        public async Task AddOrderDetailsAsync(OrderDetailsDTO OrderDetailsDTO)
        {
            var OrderDetails = _mapper.Map<OrderDetails>(OrderDetailsDTO);
            OrderDetails.TotalPriceCalculator();
            await _orderDetailsRepository.AddAsync(OrderDetails);
            OrderDetailsDTO.OrderId=OrderDetails.OrderId;
        }

        public async Task UpdateOrderDetailsAsync(OrderDetailsDTO OrderDetailsDTO, int id)
        {
            var UpdatedOrderDetails = _mapper.Map<OrderDetails>(OrderDetailsDTO);
            UpdatedOrderDetails.TotalPriceCalculator();
            await _orderDetailsRepository.UpdateAsync(id, UpdatedOrderDetails);
        }

        public async Task DeleteOrderDetailsAsync(int id)
        {
            await _orderDetailsRepository.DeleteAsync(id);
        }
        public IEnumerable<OrderDetailsDTO> GetOrderDetails(int id)
        {
            var QueryOrderDetails = _orderDetailsRepository.GetAll();
            return _mapper.Map<IEnumerable<OrderDetailsDTO>>(QueryOrderDetails);
        }
        public OrderDetailsDTO GetOrderDetailsById(int id)
        {
            var QueryOrderDetails = _orderDetailsRepository.GetAllById(id);
            return _mapper.Map<OrderDetailsDTO>(QueryOrderDetails);
        }

        public async Task<IEnumerable<OrderDetailsDTO>> SearchForOrderDetailsAsync(string SearchItem)
        {
            var SearchOrderDetails = await _orderDetailsRepository.SearchAsync(SearchItem);
            return _mapper.Map<IEnumerable<OrderDetailsDTO>>(SearchOrderDetails);

        }


    }
}
