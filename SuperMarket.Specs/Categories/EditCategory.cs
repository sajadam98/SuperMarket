using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش دسته بندی")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی  را ویرایش کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class EditCategory : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private UpdateCategoryDto _dto;
    private Category _category;
    private readonly CategoryAppService _sut;

    public EditCategory(ConfigurationFixture configuration) : base(
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
        "دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [When(
        "دسته بندی با عنوان 'لبنیات' را به دسته بندی با عنوان 'خشکبار' ویرایش میکنم")]
    public void When()
    {
        _dto = CategoryFactory.GenerateUpdateCategoryDto();

        _sut.Update(_category.Id, _dto);
    }

    [Then(
        "باید دسته بندی با عنوان 'خشکبار' در فهرست دسته بندی ها وجود داشته باشد")]
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