using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("حذف دسته بندی دارای محصول")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "دسته بندی را حذف کنم",
    InOrderTo = "کالاها را دسته بندی کنم"
)]
public class DeleteCategoryWithProduct : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private Action _expected;
    private readonly CategoryAppService _sut;

    public DeleteCategoryWithProduct(ConfigurationFixture configuration) :
        base(configuration)
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
        "دسته بندی با عنوان 'نوشیدنی' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' و حداکثر تعداد موجودی '10' جز دسته بندی 'نوشیدنی' در فهرست کالاها وجود دارد")]
    public void AndGiven()
    {
        var product = new ProductBuilder().WithCategoryId(_category.Id).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
    }

    [When("دسته بندی با عنوان 'نوشیدنی' را حذف میکنم")]
    public void When()
    {
        _expected = () => _sut.Delete(_category.Id);
    }

    [Then("باید دسته بندی با عنوان 'نوشیدنی' در فهرست دسته بندی ها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Category>().Should()
            .Contain(_ => _.Name == _category.Name);
    }
    
    [And(
        "باید خطایی با عنوان 'دسته بندی دارای کالا را نمیتوان حذف کرد'، رخ دهد")]
    public void AndThen()
    {
        _expected.Should()
            .ThrowExactly<CategoryContainsProductException>();
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => When()
            , _ => Then()
            , _ => AndThen());
    }
}