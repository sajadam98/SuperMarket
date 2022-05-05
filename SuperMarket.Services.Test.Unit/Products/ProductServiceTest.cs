
using System.Linq;
using FluentAssertions;
using Xunit;

public class ProductServiceTest 
{
    private readonly EFDataContext _dbContext;
    private readonly ProductService _sut;

    public ProductServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository repository =
            new EFProductRepository(_dbContext);
        _sut = new ProductAppService(repository,
            unitOfWork);
    }

    [Fact]
    public void Add_adds_product_properly()
    {
        var category = new Category
        {
            Name = "نوشیدنی"
        };
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var dto = new AddProductDto()
        {
            Name = "آب سیب",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = category.Id,
            MinimumAllowableStock = 0,
            MaximumAllowableStock = 10,
            Stock = 0
        };
        
        _sut.Add(dto);
        
        var expected = _dbContext.Set<Product>().FirstOrDefault();
        expected!.Name.Should().Be(dto.Name);
        expected.Price.Should().Be(dto.Price);
        expected.Stock.Should().Be(dto.Stock);
        expected.CategoryId.Should().Be(dto.CategoryId);
        expected.ProductKey.Should().Be(dto.ProductKey);
        expected.MaximumAllowableStock.Should().Be(dto.MaximumAllowableStock);
        expected.MinimumAllowableStock.Should().Be(dto.MinimumAllowableStock);
        expected.Brand.Should().Be(dto.Brand);
    }
}