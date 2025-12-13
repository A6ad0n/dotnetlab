using Moq;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Discounts.Providers;
using PizzaApp.BL.UnitTests.Helpers;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.DataAccess.Repository.DiscountRepository;

namespace PizzaApp.BL.UnitTests.Features.Discounts;

[TestFixture]
public class DiscountProviderTests
{
    private List<StatusEntity> _statusEntities;
    private List<DiscountEntity> _discountEntities;
    private Mock<IDiscountRepository> _repositoryMock;
    private IDiscountProvider _discountProvider;

    [SetUp]
    public void Setup()
    {
        _statusEntities =
        [
            new StatusEntity()
            {
                Id = 1,
                ExternalId = new Guid("10000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.DiscountActive
            },

            new StatusEntity()
            {
                Id = 2,
                ExternalId = new Guid("20000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.DiscountExpired
            }
        ];
        _discountEntities =
        [
            new DiscountEntity()
            {
                Id = 1,
                ExternalId = new Guid("00000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "The New Year Discount",
                Description = "You will be able to buy smth with discount 70%",
                DiscountPercentage = 70.0m,
                Status = _statusEntities[0]
            },

            new DiscountEntity()
            {
                Id = 2,
                ExternalId = new Guid("00000000-0000-0000-0000-000000000002"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "The Sunday Discount",
                Description = "You will be able to buy smth with discount 10%",
                DiscountPercentage = 10.0m,
                Status = _statusEntities[0]
            },

            new DiscountEntity()
            {
                Id = 3,
                ExternalId = new Guid("00000000-0000-0000-0000-000000000003"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "The Black Friday Discount",
                Description = "You will be able to buy smth with discount 90%",
                DiscountPercentage = 90.0m,
                Status = _statusEntities[1]
            }

        ];
        
        _repositoryMock = new Mock<IDiscountRepository>();
        _discountProvider =  new DiscountProvider(_repositoryMock.Object, MapperHelper.Mapper);
    }
    
    [Test]
    public async Task TestGetAllDiscounts()
    {
        _repositoryMock
            .Setup(repo => repo.GetAllWithDetailsAsync())
            .ReturnsAsync(_discountEntities);
        
        var result = await _discountProvider.GetAllAsync();
        
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(_statusEntities[0].Name.ToString(), Is.EqualTo("DiscountActive"));
        Assert.That(_statusEntities[0].Name.ToString(), Is.EqualTo("DiscountActive"));
        Assert.That(_statusEntities[1].Name.ToString(), Is.EqualTo("DiscountExpired"));
        
        _repositoryMock.Verify(repo => repo.GetAllWithDetailsAsync(), Times.Once);
    }
    
    [Test]
    public async Task TestGetByIdDiscounts_Success()
    {
        var target = _discountEntities[1];
        _repositoryMock
            .Setup(repo => repo.GetByIdWithDetailsAsync(target.Id))
            .ReturnsAsync(target);
        
        var result = await _discountProvider.GetByIdAsync(target.Id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(target.Name));
        Assert.That(result.Description, Is.EqualTo(target.Description));
        Assert.That(result.DiscountPercentage, Is.EqualTo(target.DiscountPercentage));
        Assert.That(_statusEntities[0].Name.ToString(), Is.EqualTo("DiscountActive"));
        
        _repositoryMock.Verify(repo => repo.GetByIdWithDetailsAsync(target.Id), Times.Once);
    }
    
    [Test]
    public void TestGetByIdDiscounts_ThrowsDiscountNotFound()
    {
        const int id = 999;
        _repositoryMock
            .Setup(repo => repo.GetByIdWithDetailsAsync(id))
            .ReturnsAsync((DiscountEntity?)null);
        
        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () => 
            await _discountProvider.GetByIdAsync(id));
        
        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.DiscountNotFound));
        
        _repositoryMock.Verify(repo => repo.GetByIdWithDetailsAsync(id), Times.Once);
    }
}