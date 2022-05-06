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
        ProductRepository _productRepository =
            new EFProductRepository(_dbContext);
        _sut = new CategoryAppService(repository, _productRepository,
            unitOfWork);
    }

    [Fact]
    public void Add_adds_category_properly()
    {
        var dto = CategoryFactory.GenerateAddCategoryDto();

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
        var dto = CategoryFactory.GenerateAddCategoryDto();

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<DuplicateCategoryNameInCategoryException>();
    }

    [Fact]
    public void Update_updates_category_with_id_properly()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var dto = CategoryFactory.GenerateUpdateCategoryDto();

        _sut.Update(category.Id, dto);

        _dbContext.Set<Category>().Should()
            .Contain(_ => _.Name == dto.Name);
    }

    [Fact]
    public void
        Update_throw_DuplicateCategoryNameInCategoryException_when_update_category_with_this_name_is_exist()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var category2 = CategoryFactory.GenerateCategory("خشکبار");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category2));
        var dto = CategoryFactory.GenerateUpdateCategoryDto();

        var expected = () => _sut.Update(category.Id, dto);

        expected.Should()
            .ThrowExactly<DuplicateCategoryNameInCategoryException>();
    }

    [Theory]
    [InlineData(-1)]
    public void
        Update_throw_CategoryNotExistException_when_update_category_with_given_id(
            int id)
    {
        var dto = CategoryFactory.GenerateUpdateCategoryDto();

        var expected = () => _sut.Update(id, dto);

        expected.Should()
            .ThrowExactly<CategoryNotExistException>();
    }

    [Fact]
    public void
        Delete_deletes_category_with_id_properly()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));

        _sut.Delete(category.Id);

        _dbContext.Set<Category>().Should().NotContain(_ =>
            _.Name == category.Name && _.Id == category.Id);
    }

    [Fact]
    public void
        Delete_throw_CategoryContainsProductException_with_given_id_contain_product()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new Product
        {
            Name = "انرژی زا",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = category.Id,
            MaximumAllowableStock = 10,
        };
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));

        var expected = () => _sut.Delete(category.Id);

        expected.Should().ThrowExactly<CategoryContainsProductException>();
    }

    [Fact]
    public void
        GetAll_retutns_categories_properly()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));

        var expected = _sut.GetAll();

        expected.Should().HaveCount(1);
        expected.Should().Contain(_ =>
            _.Name == category.Name && _.Id == category.Id);
    }
}