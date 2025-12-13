using PizzaApp.BL.Common.Primitives;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.BL.UnitTests.Enums;

public class PrimitivesConsistencyTests
{
    [Test]
    public void RoleEnums_ShouldMatchBetweenDALandBL()
    {
        var dalValues = Enum.GetValues<Role>().ToList();
        var blValues = Enum.GetValues<RoleTypeModel>().ToList();
        
        Assert.That(dalValues.Count, Is.EqualTo(blValues.Count), 
            "Count of elements in DAL and BL Role Enum mismatch.");

        for (var i = 0; i < dalValues.Count; ++i)
        {
            Assert.That(dalValues[i].ToString(), Is.EqualTo(blValues[i].ToString()), 
                $"The name of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
            Assert.That((int)dalValues[i], Is.EqualTo((int)blValues[i]), 
                $"The value of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
        }
    }
    
    [Test]
    public void StatusEnums_ShouldMatchBetweenDALandBL()
    {
        var dalValues = Enum.GetValues<Status>().ToList();
        var blValues = Enum.GetValues<StatusTypeModel>().ToList();
        
        Assert.That(dalValues.Count, Is.EqualTo(blValues.Count), 
            "Count of elements in DAL and BL Status Enum mismatch.");

        for (var i = 0; i < dalValues.Count; ++i)
        {
            Assert.That(dalValues[i].ToString(), Is.EqualTo(blValues[i].ToString()), 
                $"The name of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
            Assert.That((int)dalValues[i], Is.EqualTo((int)blValues[i]), 
                $"The value of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
        }
    }
    
    [Test]
    public void CategoryEnums_ShouldMatchBetweenDALandBL()
    {
        var dalValues = Enum.GetValues<MenuCategory>().ToList();
        var blValues = Enum.GetValues<CategoryTypeModel>().ToList();
        
        Assert.That(dalValues.Count, Is.EqualTo(blValues.Count), 
            "Count of elements in DAL and BL Category Enum mismatch.");

        for (var i = 0; i < dalValues.Count; ++i)
        {
            Assert.That(dalValues[i].ToString(), Is.EqualTo(blValues[i].ToString()), 
                $"The name of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
            Assert.That((int)dalValues[i], Is.EqualTo((int)blValues[i]), 
                $"The value of the enum doesn't match: {dalValues[i]} != {blValues[i]}");
        }
    }
}