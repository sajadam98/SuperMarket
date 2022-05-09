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
    private Product _product;

    public AddProductWithDuplicateProductKey(
        ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "دسته بندی با عنوان 'نوشیدنی' وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' وجود دارد")]
    public void AndGiven()
    {
        _product = new ProductBuilder().WithCategoryId(_category.Id)
            .WithMinimumAllowableStock(0).WithMaximumAllowableStock(10)
            .WithName("آب سیب").WithPrice(25000).WithProductKey("1234")
            .WithStock(0).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' تعریف میکنم")]
    public void When()
    {
        var dto = new AddProductDtoBuilder().WithStock(0)
            .WithBrand("سن ایچ").WithPrice(25000).WithName("آب سیب")
            .WithProductKey("1234").WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).Build();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);

        _expected = () => sut.Add(dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' وجود در فهرست کالاها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == _product.Brand && _.Name == _product.Name &&
            _.Price == _product.Price && _.Stock == _product.Stock &&
            _.CategoryId == _product.CategoryId &&
            _.ProductKey == _product.ProductKey &&
            _.MaximumAllowableStock == _product.MaximumAllowableStock &&
            _.MinimumAllowableStock == _product.MinimumAllowableStock);
    }

    [And(
        "و خطایی با عنوان 'کد کالا تکراری است'، رخ دهد")]
    public void AndThen()
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
            , _ => Then()
            , _ => AndThen());
    }
}