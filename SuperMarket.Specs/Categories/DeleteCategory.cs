using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("حذف دسته بندی")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی را حذف کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class DeleteCategory : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;

    public DeleteCategory(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [When("دسته بندی با عنوان 'لبنیات' را حذف میکنم")]
    public void When()
    {
        var unitOfWork = new EFUnitOfWork(_dbContext);
        CategoryRepository categoryRepository =
            new EFCategoryRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        CategoryService sut = new CategoryAppService(categoryRepository, productRepository,
            unitOfWork);

        sut.Delete(_category.Id);
    }

    [Then(
        "نباید دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Category>().Should().NotContain(_ =>
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