using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف دسته بندی")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی تعریف کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class AddCategory : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private AddCategoryDto _dto;
    private readonly CategoryAppService _sut;

    public AddCategory(ConfigurationFixture configuration) : base(
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
        "هیچ دسته بندی در فهرست دسته بندی ها وجود ندارد")]
    public void Given()
    {
    }

    [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
    public void When()
    {
        _dto = CategoryFactory.GenerateAddCategoryDto();

        _sut.Add(_dto);
    }

    [Then(
        "باید دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود داشته باشد")]
    public void Then()
    {
        var expected = _dbContext.Set<Category>().FirstOrDefault();

        expected!.Name.Should().Be(_dto.Name);
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