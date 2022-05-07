using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش دسته بندی با عنوان تکراری")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی  را ویرایش کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class
    EditCategoryWithDuplicateNameInCategory : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private Action _expected;

    public EditCategoryWithDuplicateNameInCategory(
        ConfigurationFixture configuration) : base(
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

    [And(
        "دسته بندی با عنوان 'خشکبار' در فهرست دسته بندی ها وجود دارد")]
    public void AndGiven()
    {
        var category = CategoryFactory.GenerateCategory();
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
    }

    [When(
        "دسته بندی با عنوان 'لبنیات' را به دسته بندی با عنوان 'خشکبار' ویرایش میکنم")]
    public void When()
    {
        var dto = CategoryFactory.GenerateUpdateCategoryDto();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        CategoryRepository categoryRepository =
            new EFCategoryRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        CategoryService sut = new CategoryAppService(categoryRepository,
            productRepository,
            unitOfWork);

        _expected = () => sut.Update(_category.Id, dto);
    }

    [Then(
        "باید خطایی با عنوان 'عنوان دسته بندی تکراری است'، رخ دهد")]
    public void Then()
    {
        _expected.Should()
            .ThrowExactly<DuplicateCategoryNameInCategoryException>();
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => When()
            , _ => Then());
    }
}