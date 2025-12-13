using AutoMapper;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.DataAccess.Repository.DiscountRepository;

namespace PizzaApp.BL.Features.Discounts.Providers;

public class DiscountProvider(IDiscountRepository menuItemRepository, IMapper mapper) : IDiscountProvider
{
    public async Task<DiscountModel> GetByIdAsync(int id)
    {
        var discount = await menuItemRepository.GetByIdWithDetailsAsync(id) ??
                        throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountNotFound);
        
        return mapper.Map<DiscountModel>(discount);
    }
    
    public async Task<List<DiscountModel>> GetAllAsync()
    {
        var discounts = await menuItemRepository.GetAllWithDetailsAsync();
        return mapper.Map<List<DiscountModel>>(discounts);
    }
}