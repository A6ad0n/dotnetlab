using AutoMapper;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Statuses.Entities;
using PizzaApp.BL.Features.Statuses.Exceptions;
using PizzaApp.DataAccess.Repository.StatusRepository;

namespace PizzaApp.BL.Features.Statuses.Providers;

public class StatusProvider(IStatusRepository menuItemRepository, IMapper mapper) : IStatusProvider
{
    public async Task<StatusModel> GetByIdAsync(int id)
    {
        var discount = await menuItemRepository.GetByIdWithDetailsAsync(id) ??
                        throw new BusinessLogicException<StatusResultCode>(StatusResultCode.StatusNotFound);
        
        return mapper.Map<StatusModel>(discount);
    }
    
    public async Task<List<StatusModel>> GetAllAsync()
    {
        var discounts = await menuItemRepository.GetAllWithDetailsAsync();
        return mapper.Map<List<StatusModel>>(discounts);
    }
}