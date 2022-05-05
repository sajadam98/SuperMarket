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

    public DeleteCategoryWithProduct(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "دسته بندی با عنوان 'نوشیدنی' در فهرست دسته بندی ها وجود دارد")]
    public void Given()
    {
        _category = new Category
        {
            Name = "لبنیات"
        };
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "الایی با عنوان 'انرژی زا' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' و حداکثر تعداد موجودی '10' جز دسته بندی 'نوشیدنی' در فهرست کالاها وجود دارد")]
    public void AndGiven()
    {
        var product = new Product
        {
            Name = "انرژی زا",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = _category.Id,
            MaximumAllowableStock = 10,
        };
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
    }

    [When("دسته بندی با عنوان 'نوشیدنی' را حذف میکنم")]
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

        _expected = () => sut.Delete(_category.Id);
    }

    [Then(
        "باید خطایی با عنوان 'دسته بندی دارای کالا را نمیتوان حذف کرد'، رخ دهد")]
    public void Then()
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
            , _ => Then());
    }
}