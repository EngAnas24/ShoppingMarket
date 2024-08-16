using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;

namespace ShoppingMarket.Business
{
    public class CustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> CustomerRepository, IMapper mapper)
        {
            _customerRepository = CustomerRepository;
            _mapper = mapper;
        }

       public async Task<IEnumerable<CustomerDTO>> GetCustomersAsync()
        {
            var Customer = await _customerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(Customer);
        }

        public async Task<CustomerDTO> GetCustomerAsync(int id)
        {
            var catgry = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerDTO>(catgry);

        }

        public async Task AddCustomerAsync(CustomerDTO CustomerDTO)
        {
           var Customer=  _mapper.Map<Customer>(CustomerDTO);
            await _customerRepository.AddAsync(Customer);
            CustomerDTO.Id = Customer.Id;
        }

        public async Task UpdateCustomerAsync(CustomerDTO CustomerDTO,int id)
        {
           var UpdatedCustomer = _mapper.Map<Customer>(CustomerDTO);
            await _customerRepository.UpdateAsync(id,UpdatedCustomer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteAsync(id);
        }
        public IEnumerable<CustomerDTO> GetCustomers(int id)
        {
            var QueryCustomer =  _customerRepository.GetAll();
            return _mapper.Map<IEnumerable<CustomerDTO>>(QueryCustomer);
        }
        public  CustomerDTO GetCustomerById(int id)
        {
            var QueryCustomer =  _customerRepository.GetAllById(id);
            return _mapper.Map<CustomerDTO>(QueryCustomer);
        }

        public async Task<IEnumerable<CustomerDTO>> SearchForCustomersAsync(string SearchItem)
        {
            var SearchCustomers = await _customerRepository.SearchAsync(SearchItem);
           return _mapper.Map<IEnumerable< CustomerDTO>>(SearchCustomers);
            
        }


    }
}
