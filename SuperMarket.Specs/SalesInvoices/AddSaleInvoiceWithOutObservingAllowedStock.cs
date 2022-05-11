using System;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف فاکتور فروش بدون رعایت موجودی محصول")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فاکتور فروش تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class
    AddSaleInvoiceWithOutObservingAllowedStock :
        EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly SaleInvoiceAppService _sut;
    private Product _product;
    private Action _expected;
    private AddSaleInvoiceDto _dto;

    public AddSaleInvoiceWithOutObservingAllowedStock(
        ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        _sut = new SaleInvoiceAppService(
            saleInvoiceRepository,
            productRepository,
            unitOfWork);
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '1' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        _product = new ProductBuilder().WithCategoryId(category.Id)
            .WithStock(1)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '5' با قیمت '25000' را تعریف می کنم")]
    public void When()
    {
        _dto = new AddSalesInvoiceDtoBuilder().WithPrice(25000)
            .WithCount(8).WithProductId(_product.Id)
            .WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16))
            .Build();
        
        _expected = () => _sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '60' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        _expected.Should()
            .ThrowExactly<AvailableProductStockNotObservedException>();
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