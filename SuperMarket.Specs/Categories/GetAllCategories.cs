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
    private Category _category;
    private IList<GetCategoryDto> _categories;

    public GetAllCategories(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "تنها یک دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = new Category
        {
            Name = "لبنیات"
        };
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [When("درخواست مشاهده فهرست دسته بندی را میدهم")]
    public void When()
    {
        var unitOfWork = new EFUnitOfWork(_dbContext);
        CategoryRepository categoryRepository =
            new EFCategoryRepository(_dbContext);
        ProductRepository _productRepository =
            new EFProductRepository(_dbContext);
        CategoryService sut = new CategoryAppService(categoryRepository,
            _productRepository,
            unitOfWork);

        _categories = sut.GetAll();
    }

    [Then(
        "باید دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها را مشاهده کند")]
    public void Then()
    {
        _categories.Should().Contain(_ =>
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