using PizzaApp.BL.Common.Primitives;

namespace PizzaApp.BL.Features.Menu.Entities;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryTypeModel CategoryType { get; set; }
}