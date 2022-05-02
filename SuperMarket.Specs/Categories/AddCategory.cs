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
    private readonly EFDataContext _dataContext;
    private AddCategoryDto _dto;

    public AddCategory(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dataContext = CreateDataContext();
    }

    [Given(
        "هیچ دسته بندی در فهرست دسته بندی ها وجود ندارد")]
    public void Given()
    {
    }

    [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
    public void When()
    {
        _dto = new AddCategoryDto()
        {
            Name = "لبنیات"
        };
        var unitOfWork = new EFUnitOfWork(_dataContext);
        CategoryRepository categoryRepository =
            new EFCategoryRepository(_dataContext);
        CategoryService sut = new CategoryAppService(categoryRepository,
            unitOfWork);

        sut.Add(_dto);
    }

    [Then(
        "باید دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود داشته باشد")]
    public void Then()
    {
        var expected = _dataContext.Set<Category>().FirstOrDefault();

        expected.Name.Should().Be(_dto.Name);
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