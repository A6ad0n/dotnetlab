using Microsoft.Extensions.Logging;
using Moq;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Discounts.Managers;
using PizzaApp.BL.UnitTests.Helpers;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.DataAccess.Repository.DiscountRepository;

namespace PizzaApp.BL.UnitTests.Features.Discounts;

[TestFixture]
public class DiscountManagerTests
{
    private List<StatusEntity> _statusEntities;
    private List<DiscountEntity> _discountEntities;
    private Mock<IDiscountRepository> _repo;
    private Mock<ILogger<DiscountManager>> _logger;
    private IDiscountManager _manager;

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
        
        _repo = new Mock<IDiscountRepository>();
        _logger = new Mock<ILogger<DiscountManager>>();
        _manager =  new DiscountManager(_repo.Object, MapperHelper.Mapper, _logger.Object);
    }

    [Test]
    public async Task CreateDiscountAsync_Success()
    {
        const int statusId = 1;
        const int discountId = 99;
        var model = new CreateDiscountModel
        {
            Name = "new Discount",
            Description = "new Discount",
            DiscountPercentage = 70.0m,
            StatusId = statusId
        };
        
        _repo.Setup(r => r.ExistsStatusAsync(statusId)).ReturnsAsync(true);
        _repo.Setup(r => r.SaveAsync(It.IsAny<DiscountEntity>()))
            .ReturnsAsync((DiscountEntity e) =>
            {
                e.Id = discountId;
                e.Status = _statusEntities.First(s => s.Id == e.StatusId);
                return e;
            });
        
        var result = await _manager.CreateDiscountAsync(model);
        
        Assert.That(result.Id,  Is.EqualTo(discountId));
        Assert.That(result.Name,  Is.EqualTo("new Discount"));
        Assert.That(result.Description,  Is.EqualTo("new Discount"));
        Assert.That(result.Status.Name, Is.EqualTo("DiscountActive"));
        
        _repo.Verify(r => r.ExistsStatusAsync(It.IsAny<int>()), Times.Once);
        _repo.Verify(r => r.SaveAsync(It.IsAny<DiscountEntity>()), Times.Once);
    }
    
    [Test]
    public void CreateDiscountAsync_ThrowsStatusNotFound()
    {
        const int statusId = 99;
        var model = new CreateDiscountModel
        {
            StatusId = statusId
        };
        
        _repo.Setup(r => r.ExistsStatusAsync(statusId)).ReturnsAsync(false);
        
        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () => await _manager.CreateDiscountAsync(model));
        
        Assert.That(ex.ResultCode,  Is.EqualTo(DiscountResultCode.StatusNotFound));
        
        _repo.Verify(r => r.ExistsStatusAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task UpdateDiscountAsync_Success()
    {
        const int discountId = 1;
        const int discountIndex = discountId - 1;
        var model = new UpdateDiscountModel
        {
            Name = "Updated",
            Description = "Updated",
            DiscountPercentage = 70.0m,
        };
        
        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync(_discountEntities[discountIndex]);
        _repo.Setup(r => r.SaveAsync(_discountEntities[discountIndex]))
            .ReturnsAsync(_discountEntities[discountIndex]);

        var result = await _manager.UpdateDiscountAsync(discountId, model);
        
        Assert.That(result.Name, Is.EqualTo("Updated"));
        Assert.That(result.Description, Is.EqualTo("Updated"));
        Assert.That(result.DiscountPercentage, Is.EqualTo(70.0m));
        
        _repo.Verify(r => r.GetByIdWithDetailsAsync(discountId), Times.Once);
        _repo.Verify(r => r.SaveAsync(It.IsAny<DiscountEntity>()), Times.Once);
    }

    [Test]
    public void UpdateDiscountAsync_ThrowsDiscountNotFound()
    {
        const int discountId = 99;
        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync((DiscountEntity?)null);

        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () =>
            await _manager.UpdateDiscountAsync(discountId, new UpdateDiscountModel()));
        
        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.DiscountNotFound));
    }

    [Test]
    public void UpdateDiscountAsync_ThrowsDiscountUpdateFailure()
    {
        const int discountId = 1;
        const int discountIndex = discountId - 1;
        var model = new UpdateDiscountModel { Name = "X" };
        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync(_discountEntities[discountIndex]);
        _repo.Setup(r => r.SaveAsync(_discountEntities[discountIndex]))
            .ThrowsAsync(new Exception("POSTGRESQL SMTH FAIL"));
        
        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () => 
            await _manager.UpdateDiscountAsync(discountId, model));
        
        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.DiscountUpdateFailure));
    }

    [Test]
    public async Task ChangeDiscountStatusAsync_Success()
    {
        const int discountId = 1;
        const int discountIndex = discountId - 1;
        const int statusId = 2;
        const int statusIndex = statusId - 1;
        
        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync(_discountEntities[discountIndex]);
        _repo.Setup(r => r.ExistsStatusAsync(statusId))
            .ReturnsAsync(true);

        _repo.Setup(r => r.UpdateDiscountStatusAsync(_discountEntities[discountIndex], statusId))
            .Returns(Task.CompletedTask);

        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync(() =>
            {
                _discountEntities[discountIndex].StatusId = statusId;
                _discountEntities[discountIndex].Status = _statusEntities[statusIndex];
                return _discountEntities[discountIndex];
            });
        
        var result = await _manager.ChangeDiscountStatusAsync(discountId, statusId);
        
        Assert.That(result.Id, Is.EqualTo(discountId));
        Assert.That(result.Status.Name, Is.EqualTo("DiscountExpired"));
    }

    [Test]
    public void ChangeDiscountStatusAsync_ThrowsDiscountNotFound()
    {
        const int discountId = 99;
        
        _repo.Setup(r => r.GetByIdWithDetailsAsync(discountId))
            .ReturnsAsync((DiscountEntity?)null);

        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () =>
            await _manager.ChangeDiscountStatusAsync(discountId, 1));
        
        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.DiscountNotFound));
    }

    [Test]
    public void ChangeDiscountStatusAsync_ThrowsStatusNotFound()
    {
        _repo.Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(_discountEntities[0]);
        _repo.Setup(r => r.ExistsStatusAsync(999)).ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () =>
            await _manager.ChangeDiscountStatusAsync(1, 999));
        
        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.StatusNotFound));
    }

    [Test]
    public void ChangeDiscountStatusAsync_ThrowsStatusUpdateFailure()
    {
        _repo.Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(_discountEntities[0]);
        _repo.Setup(r => r.ExistsStatusAsync(2)).ReturnsAsync(true);
        
        _repo.Setup(r => r.UpdateDiscountStatusAsync(_discountEntities[0], 2))
            .ThrowsAsync(new Exception("POSTGRESQL SMTH FAIL"));

        var ex = Assert.ThrowsAsync<BusinessLogicException<DiscountResultCode>>(async () =>
            await _manager.ChangeDiscountStatusAsync(1, 2));

        Assert.That(ex.ResultCode, Is.EqualTo(DiscountResultCode.DiscountUpdateFailure));
    }


    [Test]
    public async Task DeleteDiscountAsync_Success()
    {
        _repo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        var result = await _manager.DeleteDiscountAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteDiscountAsync_Failure()
    {
        _repo.Setup(r => r.DeleteAsync(100)).ReturnsAsync(false);
        
        var result =  await _manager.DeleteDiscountAsync(100);

        Assert.That(result, Is.False);
    }
}