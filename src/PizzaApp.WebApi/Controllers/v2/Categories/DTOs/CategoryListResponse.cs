using PizzaApp.BL.Features.Categories.Entities;

namespace PizzaApp.WebApi.Controllers.v2.Categories.DTOs;

public class CategoryListResponse
{
    public List<CategoryModel> Categories { get; set; }
}