using AutoMapper;
using ShoppingMarket.Data;
using ShoppingMarket.Models;
using ShoppingMarket.Models.DTOS;

namespace ShoppingMarket.Business
{
    public class CategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task AddCategoryAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.AddAsync(category);
            categoryDTO.Id = category.Id; 
        }

        public async Task<CategoryDTO> GetCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO,int id)
        {
           var UpdatedCategory = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.UpdateAsync(id,UpdatedCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
        public IEnumerable<CategoryDTO> GetCategories(int id)
        {
            var QueryCategory =  _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDTO>>(QueryCategory);
        }
        public  CategoryDTO GetCategoryById(int id)
        {
            var QueryCategory =  _categoryRepository.GetAllById(id);
            return _mapper.Map<CategoryDTO>(QueryCategory);
        }

        public async Task<IEnumerable<CategoryDTO>> SearchForCategoriesAsync(string SearchItem)
        {
            var SearchCategories = await _categoryRepository.SearchAsync(SearchItem);
           return _mapper.Map<IEnumerable< CategoryDTO>>(SearchCategories);
            
        }


    }
}
