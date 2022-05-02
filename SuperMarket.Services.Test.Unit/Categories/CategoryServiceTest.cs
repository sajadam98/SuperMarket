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

    [Fact]
    public void
        Add_throw_DuplicateCategoryNameInCategoryException_when_add_category_with_this_name_is_exist()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var dto = CategoryFactory.GenerateCategoryDto();

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateCategoryNameInCategoryException>();
    }
}