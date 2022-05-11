using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست دسته بندی ها")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "فهرست دسته بندی ها را مشاهده کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class GetAllCategories : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly CategoryAppService _sut;
    private Category _category;
    private IList<GetCategoryDto> _expected;

    public GetAllCategories(ConfigurationFixture configuration) : base(
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
        "تنها یک دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [When("درخواست مشاهده فهرست دسته بندی را میدهم")]
    public void When()
    {
        _expected = _sut.GetAll();
    }

    [Then(
        "باید دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها را مشاهده کند")]
    public void Then()
    {
        _expected.Should().HaveCount(1);
        _expected.Should().Contain(_ =>
            _.Name == _category.Name && _.Id == _category.Id);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => When()
            , _ => Then());
    }
}