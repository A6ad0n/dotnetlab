using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;
using PizzaApp.BL.Features.Menu.Exceptions;
using PizzaApp.BL.Features.Menu.Validators;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Repository.MenuItemRepository;

namespace PizzaApp.BL.Features.Menu.Managers;

public class MenuManager(IMenuItemRepository menuItemRepository, IMapper mapper, ILogger<MenuManager> logger)
    : IMenuManager
{
    public async Task<MenuItemModel> CreateMenuItemAsync(CreateMenuItemModel model)
    {
        var validationResult = await new CreateMenuItemModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemValidationFailure, 
                stringBuilder.ToString());
        }
        
        var menuItem = mapper.Map<MenuItemEntity>(model);

        var isCategoryExist = await menuItemRepository.ExistsCategoryAsync(menuItem.CategoryId);
        if (!isCategoryExist)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.CategoryNotFound);
        }
        var isStatusExist = await menuItemRepository.ExistsStatusAsync(menuItem.StatusId);
        if (!isStatusExist)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.StatusNotFound);
        }

        if (model.DiscountIds != null)
        {
            await menuItemRepository.SaveWithDiscountsAsync(menuItem, model.DiscountIds);
        }

        var newMenuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItem.Id);
        return mapper.Map<MenuItemModel>(newMenuItem);
    }

    public async Task<MenuItemModel> UpdateMenuItemAsync(int menuItemId, UpdateMenuItemModel model)
    {
        var validationResult = await new UpdateMenuItemModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemValidationFailure, 
                stringBuilder.ToString());
        }
        
        var menuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId) ?? 
                   throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            menuItem.Name = model.Name;
        }
        if (!string.IsNullOrWhiteSpace(model.Description))
        {
            menuItem.Description = model.Description;
        }
        if (!string.IsNullOrWhiteSpace(model.ImageUrl))
        {
            menuItem.ImageUrl = model.ImageUrl;
        }
        if (model.Price is > 0)
        {
            menuItem.Price  = model.Price.Value;
        }

        try
        {
            var updatedMenuItem = await menuItemRepository.SaveAsync(menuItem);
            return mapper.Map<MenuItemModel>(updatedMenuItem);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemUpdateFailure);
        }
    }

    public async Task<MenuItemModel> ChangeMenuItemCategoryAsync(int menuItemId, int categoryId)
    {
        var menuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId) ??
                   throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        var isCategoryExist = await menuItemRepository.ExistsCategoryAsync(categoryId);

        if (!isCategoryExist)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.CategoryNotFound);
        }
        
        try
        {
            await menuItemRepository.UpdateMenuItemCategoryAsync(menuItem, categoryId);
            
            var updatedMenuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId);
            return mapper.Map<MenuItemModel>(updatedMenuItem);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemUpdateFailure, "Failed to update menuItem block information");
        }
    }
    
    public async Task<MenuItemModel> ChangeMenuItemStatusAsync(int menuItemId, int statusId)
    {
        var menuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId) ??
                       throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        var isStatusExist = await menuItemRepository.ExistsStatusAsync(statusId);

        if (!isStatusExist)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.StatusNotFound);
        }
        
        try
        {
            await menuItemRepository.UpdateMenuItemStatusAsync(menuItem, statusId);
            
            var updatedMenuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId);
            return mapper.Map<MenuItemModel>(updatedMenuItem);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemUpdateFailure, "Failed to update menuItem block information");
        }
    }
    
    public async Task<MenuItemModel> ChangeMenuItemDiscountsAsync(int menuItemId, List<int> discountIds)
    {
        var menuItem = await menuItemRepository.GetByIdWithDiscountsAsync(menuItemId)
                   ?? throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);

        try
        {
            var allDiscounts = await menuItemRepository.GetAllDiscountsAsync();
            var allDiscountIds = allDiscounts.Select(r => r.Id).ToHashSet();
            
            
            var nonExistent = discountIds.Where(r => !allDiscountIds.Contains(r)).ToList();
            if (nonExistent.Count != 0)
            {
                throw new BusinessLogicException<MenuResultCode>(
                    MenuResultCode.DiscountsNotFound,
                    $"Non-existent discounts: {string.Join(", ", nonExistent)}"
                );
            }
            
            var newDiscountIds = discountIds.Where(r => allDiscountIds.Contains(r)).ToList();
            
            await menuItemRepository.UpdateMenuItemDiscountsAsync(menuItem, newDiscountIds);

            var updatedMenuItem = await menuItemRepository.GetByIdWithDetailsAsync(menuItemId);
            return mapper.Map<MenuItemModel>(updatedMenuItem);
        }
        catch (Exception e) when (e is not BusinessLogicException<MenuResultCode>)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemUpdateFailure, "Failed to update menuItem's discounts");
        }
    }
    
    public async Task<MenuItemModel> UpdateMenuItemAsync(Guid menuItemGuid, UpdateMenuItemModel model)
    {
        var menuItem = await menuItemRepository.GetByGuidWithDetailsAsync(menuItemGuid) ?? 
                   throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        return await UpdateMenuItemAsync(menuItem.Id, model);
    }

    public async Task<MenuItemModel> ChangeMenuItemCategoryAsync(Guid menuItemGuid, Guid categoryGuid)
    {
        var menuItem = await menuItemRepository.GetByGuidWithDetailsAsync(menuItemGuid) ??
                   throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        var categories = await menuItemRepository.GetAllCategoriesAsync();
        var category = categories.FirstOrDefault(c => c.ExternalId == categoryGuid);
        if (category == null)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.CategoryNotFound);
        }

        return await ChangeMenuItemCategoryAsync(menuItem.Id, category.Id);
    }
    
    public async Task<MenuItemModel> ChangeMenuItemStatusAsync(Guid menuItemGuid, Guid statusGuid)
    {
        var menuItem = await menuItemRepository.GetByGuidWithDetailsAsync(menuItemGuid) ??
                       throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        var statuses = await menuItemRepository.GetAllStatusesAsync();
        var status = statuses.FirstOrDefault(s => s.ExternalId == statusGuid);
        if (status == null)
        {
            throw new BusinessLogicException<MenuResultCode>(MenuResultCode.StatusNotFound);
        }
        
        return await ChangeMenuItemStatusAsync(menuItem.Id, status.Id);
    }
    
    public async Task<MenuItemModel> ChangeMenuItemDiscountsAsync(Guid menuItemGuid, List<Guid> discountGuids)
    {
        var menuItem = await menuItemRepository.GetByGuidWithDiscountsAsync(menuItemGuid)
                   ?? throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        var discounts = await menuItemRepository.GetAllDiscountsAsync();
        var discountIds = discounts
            .Where(d => discountGuids.Contains(d.ExternalId))
            .Select(d => d.Id)
            .ToList();
        
        return await ChangeMenuItemDiscountsAsync(menuItem.Id, discountIds);
    }
    
    public async Task<bool> DeleteMenuItemAsync(int menuItemId)
    {
        return await menuItemRepository.DeleteAsync(menuItemId);
    }
    
    public async Task<bool> DeleteMenuItemAsync(Guid menuItemGuid)
    {
        return await menuItemRepository.DeleteAsync(menuItemGuid);
    }
}