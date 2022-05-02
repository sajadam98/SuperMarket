using FluentAssertions;
using Xunit;

public class CategoryServiceTest
{
    private readonly EFDataContext _dbContext;
    private readonly CategoryService _sut;

    public CategoryServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        CategoryRepository repository =
            new EFCategoryRepository(_dbContext);
        _sut = new CategoryAppService(repository, unitOfWork);
    }

    [Fact]
    public void Add_adds_category_properly()
    {
        var dto = CategoryFactory.GenerateCategoryDto();

        _sut.Add(dto);

        _dbContext.Set<Category>().Should()
            .Contain(_ => _.Name == dto.Name);
    }
}