using System;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده گزارش سود و زیاد")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "گزارش سود و زیاد را در بازه زمانی مشاهده کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetProfitAndLossReport : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly ProductAppService _sut;
    private Product _product;
    private int _expected;
    private EntryDocument _entryDocument;
    private SalesInvoice _saleInvoice;

    public GetProfitAndLossReport(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        _sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);
    }

    [Given(
        "تنها یک کالا با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '10' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory();
        _product = new ProductBuilder().WithStock(10).WithName("آب سیب")
            .WithCategory(category).WithPrice(2500).WithProductKey("1234")
            .WithMinimumAllowableStock(0).WithMaximumAllowableStock(10)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "تنها یک فاکتور با تاریخ صدور '19/04/1400' و نام خرید کننده 'علی علینقیپور' و تعداد خرید '2' با قیمت '25000' و شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' درفهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _saleInvoice = new SalesInvoiceBuilder().WithProduct(_product)
            .WithPrice(25000).WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16))
            .WithCount(2).Build();
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(_saleInvoice));
    }

    [And(
        "تنها یک سند با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود دارد")]
    public void AndGiven2()
    {
        _entryDocument =
            new EntryDocumentBuilder().WithCount(50)
                .WithPurchasePrice(18000)
                .WithExpirationDate(new DateTime(1900, 10, 16))
                .WithManufactureDate(new DateTime(1900, 04, 16))
                .WithDateTime(new DateTime(1900, 04, 16))
                .WithProductId(_product.Id).Build();
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "درخواست مشاهده گزارش سود و زیاد را میدهم.")]
    public void When()
    {
        _expected = _sut.GetProfitAndLossReport();
    }

    [Then("باید پیغامی با عنوان '14000+' مشاهده کنم")]
    public void Then()
    {
        _expected.Should().Be(_saleInvoice.Count * _saleInvoice.Price -
                              _entryDocument.Count *
                              _entryDocument.PurchasePrice);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => AndGiven2()
            , _ => When()
            , _ => Then());
    }
}