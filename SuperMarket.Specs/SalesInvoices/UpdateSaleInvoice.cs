using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش فاکتور فروش")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فاکتور فروش را ویرایش کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class UpdateSaleInvoice : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly SaleInvoiceAppService _sut;
    private Product _product;
    private SalesInvoice _salesInvoice;
    private UpdateSaleInvoiceDto _dto;

    public UpdateSaleInvoice(ConfigurationFixture configuration) : base(
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
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithStock(10).WithName("آب سیب")
            .WithProductKey("1234").WithPrice(25000).WithCategory(category)
            .WithMaximumAllowableStock(10).WithMinimumAllowableStock(0)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _salesInvoice = new SalesInvoiceBuilder().WithProduct(_product)
            .WithCount(2).WithPrice(25000).WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16))
            .Build();
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(_salesInvoice));
    }

    [When(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' را به فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '8' با قیمت '24000' ویرایش می کنم")]
    public void When()
    {
        _dto = new UpdateSalesInvoiceDtoBuilder().WithPrice(24000)
            .WithCount(8).WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16))
            .WithProductId(_product.Id)
            .Build();

        _sut.Update(_salesInvoice.Id, _dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '4' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == _product.Brand && _.Id == _product.Id &&
            _.Name == _product.Name && _.Price == _product.Price &&
            _.Stock == _product.Stock &&
            _.CategoryId == _product.CategoryId &&
            _.ProductKey == _product.ProductKey &&
            _.MaximumAllowableStock == _product.MaximumAllowableStock &&
            _.MinimumAllowableStock == _product.MinimumAllowableStock);
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '8' با قیمت '24000'  در فهرست فاکتورها وجود داشته باشد")]
    public void AndThen()
    {
        var expected = _dbContext.Set<SalesInvoice>()
            .FirstOrDefault(_ => _.Id == _salesInvoice.Id);
        expected!.Count.Should().Be(_dto.Count);
        expected.BuyerName.Should().Be(_dto.BuyerName);
        expected.Price.Should().Be(_dto.Price);
        expected.DateTime.Should().Be(_dto.DateTime);
        expected.ProductId.Should().Be(_dto.ProductId);
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