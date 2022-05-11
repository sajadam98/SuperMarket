using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست سندها")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetAllSaleInvoices : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly SaleInvoiceAppService _sut;
    private IList<GetSaleInvoiceDto> _expected;
    private SalesInvoice _salesInvoice;
    private Product _product;

    public GetAllSaleInvoices(ConfigurationFixture configuration) : base(
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

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و جز دسته بندی 'نوشیدنی' , حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '10' و تعداد موجودی '10' و با قیمت '25000' در فهرست کالاها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithCategory(category)
            .WithStock(10).WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).WithName("آب سیب")
            .WithProductKey("1234").WithPrice(25000).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [Given(
        "تنها یک فاکتور با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _salesInvoice = new SalesInvoiceBuilder().WithCount(2)
            .WithPrice(25000).WithProduct(_product)
            .WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16)).Build();
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(_salesInvoice));
    }

    [When("درخواست مشاهده فهرست فاکتورها را میدهم")]
    public void When()
    {
        _expected = _sut.GetAll();
    }

    [Then(
        "باید فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000'  را مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(1);
        _expected.Should().Contain(_ =>
            _.Count == _salesInvoice.Count &&
            _.Product.Id == _salesInvoice.ProductId &&
            _.Price == _salesInvoice.Price &&
            _.BuyerName == _salesInvoice.BuyerName &&
            _.DateTime == _salesInvoice.DateTime && _.TotalPrice ==
            _salesInvoice.Count * _salesInvoice.Price);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => When()
            , _  => Then());
    }
}