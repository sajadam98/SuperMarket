using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
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
    private Product _product;
    private int _expected;
    private EntryDocument _entryDocument;
    private SalesInvoice _saleInvoice;

    public GetProfitAndLossReport(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "تنها یک کالا با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '10' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory();
        _product = new ProductBuilder().Build();
        _product.Category = category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "تنها یک فاکتور با تاریخ صدور '19/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' درفهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _saleInvoice = SaleInvoiceFactory.GenerateSaleInvoice();
        _saleInvoice.Product = _product;
        _saleInvoice.Count = 2;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(_saleInvoice));
    }

    [And(
        "تنها یک سند با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود دارد")]
    public void AndGiven2()
    {
        _entryDocument =
            EntryDocumentFactory.GenerateEntryDocument(_product.Id);
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "درخواست مشاهده گزارش سود و زیاد را میدهم.")]
    public void When()
    {
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository repository = new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(repository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);


        _expected = sut.GetProfitAndLossReport();
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
            ,_ => When()
            ,_ => Then());
    }
}