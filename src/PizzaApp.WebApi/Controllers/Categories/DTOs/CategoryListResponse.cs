using PizzaApp.BL.Features.Categories.Entities;

namespace PizzaApp.WebApi.Controllers.Categories.DTOs;

public class CategoryListResponse
{
    public List<CategoryModel> Categories { get; set; }
}