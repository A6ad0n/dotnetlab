using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Discounts.Validators;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Repository.DiscountRepository;

namespace PizzaApp.BL.Features.Discounts.Managers;

public class DiscountManager(IDiscountRepository discountRepository, IMapper mapper, ILogger<DiscountManager> logger)
    : IDiscountManager
{
    public async Task<DiscountModel> CreateDiscountAsync(CreateDiscountModel model)
    {
        var discount = mapper.Map<DiscountEntity>(model);
        
        var isStatusExist = await discountRepository.ExistsStatusAsync(discount.StatusId);
        if (!isStatusExist)
        {
            throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.StatusNotFound);
        }
        
        var newDiscount = await discountRepository.SaveAsync(discount);
        return mapper.Map<DiscountModel>(newDiscount);
    }

    public async Task<DiscountModel> UpdateDiscountAsync(int discountId, UpdateDiscountModel model)
    {
        var validationResult = await new UpdateDiscountModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountValidationFailure,
                stringBuilder.ToString());
        }
        
        var discount = await discountRepository.GetByIdWithDetailsAsync(discountId) ?? 
                   throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountNotFound);
        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            discount.Name = model.Name;
        }
        if (!string.IsNullOrWhiteSpace(model.Description))
        {
            discount.Description = model.Description;
        }
        if (model.ValidFrom != null && model.ValidFrom < discount.ValidTo)
        {
            discount.ValidFrom = model.ValidFrom.Value;
        }
        if (model.ValidTo != null && model.ValidTo > discount.ValidFrom)
        {
            discount.ValidTo = model.ValidTo.Value;
        }
        if (model.DiscountPercentage is > 0.0m and <= 100.0m)
        {
            discount.DiscountPercentage  = model.DiscountPercentage.Value;
        }

        try
        {
            var updatedDiscount = await discountRepository.SaveAsync(discount);
            return mapper.Map<DiscountModel>(updatedDiscount);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountUpdateFailure);
        }
    }
    
    public async Task<DiscountModel> ChangeDiscountStatusAsync(int discountId, int statusId)
    {
        var discount = await discountRepository.GetByIdWithDetailsAsync(discountId) ??
                       throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountNotFound);
        var isStatusExist = await discountRepository.ExistsStatusAsync(statusId);

        if (!isStatusExist)
        {
            throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.StatusNotFound);
        }
        
        try
        {
            await discountRepository.UpdateDiscountStatusAsync(discount, statusId);
            
            var updatedDiscount = await discountRepository.GetByIdWithDetailsAsync(discountId);
            return mapper.Map<DiscountModel>(updatedDiscount);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<DiscountResultCode>(DiscountResultCode.DiscountUpdateFailure, 
                "Failed to update discount block information");
        }
    }

    public async Task<bool> DeleteDiscountAsync(int discountId)
    {
        return await discountRepository.DeleteAsync(discountId);
    }
}