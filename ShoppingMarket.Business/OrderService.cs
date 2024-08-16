using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;

namespace ShoppingMarket.Business
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IRepository<Order> OrderRepository, IMapper mapper)
        {
            _orderRepository = OrderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
        {
            var Order = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(Order);
        }

        public async Task<OrderDTO> GetOrderAsync(int id)
        {
            var catgry = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(catgry);

        }

        public async Task AddOrderAsync(OrderDTO OrderDTO)
        {
            var Order = _mapper.Map<Order>(OrderDTO);
            foreach (var details in Order.OrderDetails)
            {
                details.TotalPriceCalculator();
            }
            await _orderRepository.AddAsync(Order);
            OrderDTO.Id = Order.Id;
            foreach (var details in OrderDTO.OrderDetailsDto)
            {
                details.OrderId = OrderDTO.Id;
                details.TotalPriceCalculator();
            }
            
        }

        public async Task UpdateOrderAsync(OrderDTO OrderDTO, int id)
        {
            var UpdatedOrder = _mapper.Map<Order>(OrderDTO);
            foreach (var details in UpdatedOrder.OrderDetails)
            {
                details.TotalPriceCalculator();
            }
            await _orderRepository.UpdateAsync(id, UpdatedOrder);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }
        public IEnumerable<OrderDTO> GetOrder(int id)
        {
            var QueryOrder = _orderRepository.GetAll();
            return _mapper.Map<IEnumerable<OrderDTO>>(QueryOrder);
        }
        public OrderDTO GetOrderById(int id)
        {
            var QueryOrder = _orderRepository.GetAllById(id);
            return _mapper.Map<OrderDTO>(QueryOrder);
        }

        public async Task<IEnumerable<OrderDTO>> SearchForOrdersAsync(string SearchItem)
        {
            var SearchOrders = await _orderRepository.SearchAsync(SearchItem);
            return _mapper.Map<IEnumerable<OrderDTO>>(SearchOrders);

        }
        public IEnumerable<OrderDTO>OrderCart(int CustomerId,int OrderId)
        {
            var SearchOrders =  _orderRepository.GetAll().Where(x=>x.CustomerId==CustomerId&&x.Id== OrderId);
            return _mapper.Map<IEnumerable<OrderDTO>>(SearchOrders);

        }

    }
}
