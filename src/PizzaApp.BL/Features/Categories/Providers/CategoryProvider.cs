using AutoMapper;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Categories.Entities;
using PizzaApp.BL.Features.Categories.Exceptions;
using PizzaApp.DataAccess.Repository.CategoryRepository;

namespace PizzaApp.BL.Features.Categories.Providers;

public class CategoryProvider(ICategoryRepository menuItemRepository, IMapper mapper) : ICategoryProvider
{
    public async Task<CategoryModel> GetByIdAsync(int id)
    {
        var discount = await menuItemRepository.GetByIdWithDetailsAsync(id) ??
                        throw new BusinessLogicException<CategoryResultCode>(CategoryResultCode.CategoryNotFound);
        
        return mapper.Map<CategoryModel>(discount);
    }
    
    public async Task<List<CategoryModel>> GetAllAsync()
    {
        var discounts = await menuItemRepository.GetAllWithDetailsAsync();
        return mapper.Map<List<CategoryModel>>(discounts);
    }
}