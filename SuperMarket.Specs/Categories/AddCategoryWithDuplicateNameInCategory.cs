using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("⦁	تعریف دسته بندی با عنوان تکراری")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی تعریف کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class
    AddCategoryWithDuplicateNameInCategory : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private AddCategoryDto _dto;
    private Category _category;
    private Action _expected;
    private readonly CategoryAppService _sut;

    public AddCategoryWithDuplicateNameInCategory(
        ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        CategoryRepository categoryRepository =
            new EFCategoryRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        _sut = new CategoryAppService(categoryRepository,
            productRepository,
            unitOfWork);
    }

    [Given(
        "دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود داشته باشد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
    public void When()
    {
        _dto = CategoryFactory.GenerateAddCategoryDto();

        _expected = () => _sut.Add(_dto);
    }

    [Then("باید تنها یک دسته بندی با عنوان 'لبنیات' وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Category>().Where(_ => _.Name == _category.Name)
            .Should().HaveCount(1);
    }

    [And("باید خطایی با عنوان 'عنوان دسته بندی تکراری است'، رخ دهد")]
    public void ThenAnd()
    {
        _expected.Should()
            .ThrowExactly<DuplicateCategoryNameInCategoryException>();
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => When()
            , _ => Then()
            , _ => ThenAnd());
    }
}