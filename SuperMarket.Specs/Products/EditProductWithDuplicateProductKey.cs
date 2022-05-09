using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش کالا با کد کالا تکراری")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "کالا را ویرایش کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class
    EditProductWithDuplicateProductKey : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private Product _product;
    private Action _expected;

    public EditProductWithDuplicateProductKey(
        ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10'  و تعداد موجودی '0' وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithCategory(_category).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "کالایی با عنوان 'آب انبه' و کدکالا '4321' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10'  و تعداد موجودی '0' وجود دارد")]
    public void AndGiven()
    {
        var product = new ProductBuilder().WithCategoryId(_category.Id)
            .WithProductKey("4321").WithName("آب انبه").WithStock(0)
            .WithMinimumAllowableStock(0).WithMaximumAllowableStock(10)
            .WithProductKey("4321").WithPrice(2500).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '0' را به کالایی با عنوان 'آب آلبالو' و کدکالا '4321' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' ویرایش میکنم")]
    public void When()
    {
        var dto =
            new UpdateProductDtoBuilder().WithStock(0)
                .WithMinimumAllowableStock(0)
                .WithMaximumAllowableStock(10).WithName("آب آلبالو")
                .WithCategoryId(_category.Id).WithProductKey("4321")
                .WithPrice(25000).Build();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);

        _expected = () => sut.Update(_product.Id, dto);
    }

    [Then("")]
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
        "باید خطایی با عنوان 'کد کالا تکراری است'، رخ دهد")]
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