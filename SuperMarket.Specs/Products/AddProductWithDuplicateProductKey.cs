using System;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف کالا با کد تکراری")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "کالا را تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class
    AddProductWithDuplicateProductKey : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private Action _expected;

    public AddProductWithDuplicateProductKey(
        ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "دسته بندی با عنوان 'نوشیدنی' وجود دارد")]
    public void Given()
    {
        _category = new Category
        {
            Name = "نوشیدنی"
        };
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' وجود دارد")]
    public void AndGiven()
    {
        var product = new Product
        {
            Name = "آب سیب",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = _category.Id,
            MinimumAllowableStock = 0,
            MaximumAllowableStock = 10,
            Stock = 0
        };
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' تعریف میکنم")]
    public void When()
    {
        var dto = new AddProductDto
        {
            Name = "آب سیب",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = _category.Id,
            MinimumAllowableStock = 0,
            MaximumAllowableStock = 10,
            Stock = 0
        };
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            unitOfWork);

        _expected = () => sut.Add(dto);
    }

    [Then(
        "باید خطایی با عنوان 'کد کالا تکراری است'، رخ دهد")]
    public void Then()
    {
        _expected.Should().ThrowExactly<DuplicateProductKeyException>();
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