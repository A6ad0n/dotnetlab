using Microsoft.EntityFrameworkCore;
using Moq;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.DataAccess.Repository.DiscountRepository;

namespace PizzaApp.BL.UnitTests.Features.Discounts;

[TestFixture]
public class DiscountRepositoryTests
{
    private DbContextOptions<PizzaAppDbContext> _dbOptions;
    private Mock<IDbContextFactory<PizzaAppDbContext>> _factoryMock;
    private PizzaAppDbContext _context;
    private DiscountRepository _repo;

    [SetUp]
    public async Task Setup()
    {
        _dbOptions = new DbContextOptionsBuilder<PizzaAppDbContext>()
            .UseInMemoryDatabase(databaseName: $"PizzaApp_{System.Guid.NewGuid()}")
            .Options;

        _context = new PizzaAppDbContext(_dbOptions);

        _factoryMock = new Mock<IDbContextFactory<PizzaAppDbContext>>();
        _factoryMock
            .Setup(f => f.CreateDbContextAsync(It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(() => new PizzaAppDbContext(_dbOptions));

        var statuses = new[]
        {
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
            },
            new StatusEntity()
            {
                Id = 3,
                ExternalId = new Guid("30000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.MenuActive
            }
        };

        var categories = new[]
        {
            new MenuCategoryEntity()
            {
                Id = 1,
                ExternalId = new Guid("11000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = MenuCategory.Drink
            },
            new MenuCategoryEntity()
            {
                Id = 2,
                ExternalId = new Guid("22000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = MenuCategory.Pizza
            }
        };

        var menuItems = new[]
        {
            new MenuItemEntity()
            {
                Id = 10,
                ExternalId = new Guid("11100000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Pizza",
                Description = "Pizza with smth delicious",
                ImageUrl = "smthlikeurl",
                Price = 100,
                CategoryId = 2,
                StatusId = 3
            },
            new MenuItemEntity()
            {
                Id = 20,
                ExternalId = new Guid("22200000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Drink",
                Description = "Drink with smth delicious",
                ImageUrl = "smthlikeurl",
                Price = 20,
                CategoryId = 1,
                StatusId = 3
            }
        };

        var discounts = new[]
        {
            new DiscountEntity()
            {
                Id = 1,
                ExternalId = new Guid("11110000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Mega Discount",
                Description = "Mega Discount ESKETIT",
                DiscountPercentage = 90.0m,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(10),
                StatusId = 1,
                MenuItems = new List<MenuItemDiscountEntity>
                {
                    new MenuItemDiscountEntity { MenuItemId = 10, DiscountId = 1 }
                }
            },
            new DiscountEntity()
            {
                Id = 2,
                ExternalId = new Guid("22220000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Mega Summer Discount",
                Description = "Mega Discount ESKETIT",
                DiscountPercentage = 15.0m,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(20),
                StatusId = 2,
                MenuItems = new List<MenuItemDiscountEntity>()
            }
        };
        
        _context.Statuses.AddRange(statuses);
        _context.MenuCategories.AddRange(categories);
        _context.MenuItems.AddRange(menuItems);
        _context.Discounts.AddRange(discounts);
        
        await _context.SaveChangesAsync();
        
        _repo = new DiscountRepository(_factoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    [Test]
    public async Task GetByIdWithDetailsAsync_Success()
    {
        var d = await _repo.GetByIdWithDetailsAsync(1);

        Assert.That(d, Is.Not.Null);
        Assert.That(d.Name, Is.EqualTo("Mega Discount"));
        Assert.That(d.Status, Is.Not.Null);
        Assert.That(d.MenuItems.Count, Is.EqualTo(1));
        Assert.That(d.MenuItems.First().MenuItemId, Is.EqualTo(10));
    }

    [Test]
    public async Task GetByIdWithStatusASync_Success()
    {
        var d = await _repo.GetByIdWithStatusAsync(1);

        Assert.That(d, Is.Not.Null);
        Assert.That(d.Status, Is.Not.Null);
        Assert.That(d.Status.Name, Is.EqualTo(Status.DiscountActive));
    }

    [Test]
    public async Task GetAllWithDetailsAsync_Success()
    {
        var list = await _repo.GetAllWithDetailsAsync();
        
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0].Status, Is.Not.Null);
    }

    [Test]
    public async Task GetDiscountsByStatusAsync_Success()
    {
        var list = await _repo.GetDiscountsByStatusAsync(Status.DiscountActive);
        
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Status.Name, Is.EqualTo(Status.DiscountActive));
    }
    
    [Test]
    public async Task GetDiscountsByStatusIdAsync_Success()
    {
        var list = await _repo.GetDiscountsByStatusIdAsync(2);
        
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list[0].Status.Name, Is.EqualTo(Status.DiscountExpired));
    }

    [Test]
    public async Task GetDiscountsPagedAsync_Success()
    {
        var (items, total) = await _repo.GetDiscountsPagedAsync(
            pageNumber: 1,
            pageSize: 1,
            searchTerm: "e",
            sortBy: "name",
            ascending: true);
        
        Assert.That(items, Is.Not.Null);
        Assert.That(items.Count, Is.EqualTo(1));
        Assert.That(total, Is.EqualTo(2));
        Assert.That(items[0].Name, Is.EqualTo("Mega Discount"));
    }

    [Test]
    public async Task GetDiscountsCountAsync_Success()
    {
        var count = await _repo.GetDiscountsCountAsync();
        
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public async Task ExistsStatusAsync_Success()
    {
        var r = await _repo.ExistsStatusAsync(1);
        
        Assert.That(r, Is.True);
    }

    [Test]
    public async Task ExistsStatusAsync_Failure()
    {
        var  r = await _repo.ExistsStatusAsync(999);
        Assert.That(r, Is.False);
    }

    [Test]
    public async Task GetAllstatusesAsync_Success()
    {
        var list = await _repo.GetAllStatusesAsync();
        
        Assert.That(list.Count, Is.EqualTo(3));
    }

    public async Task UpdateDiscountStatusAsync_Success()
    {
        var discount = (await _repo.GetAllWithDetailsAsync()).First();

        await _repo.UpdateDiscountStatusAsync(discount, 2);
        
        var updated = await _repo.GetByIdWithStatusAsync(discount.Id);
        
        Assert.That(updated.StatusId, Is.EqualTo(2));
        Assert.That(updated.Status.Name, Is.EqualTo(Status.DiscountExpired));
    }
}