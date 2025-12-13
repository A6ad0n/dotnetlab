using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Menu.Exceptions;
using PizzaApp.BL.Features.Users.Exceptions;

namespace PizzaApp.BL.UnitTests.Enums;

[TestFixture]
public class ResultCodesConsistencyTests
{
    [Test]
    public void ResultCodes_ShouldBeUnique()
    {
        var allValues = new List<int>();

        AddEnumValues<CommonResultCode>();
        AddEnumValues<DiscountResultCode>();
        AddEnumValues<MenuResultCode>();
        AddEnumValues<UserResultCode>();
        
        var duplicates = allValues
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        
        Assert.That(duplicates, Is.Empty,
            "Found duplicate values: " + string.Join(", ", duplicates));
        return;

        void AddEnumValues<T>() where T : Enum
        {
            allValues.AddRange(Enum.GetValues(typeof(T)).Cast<int>());
        }
    }
}