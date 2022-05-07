using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست کالاهای کم مشتری")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فهرست کالاهای پرفروش را مشاهده کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetBestSellersProducts : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private IList<GetProductDto> _expected;
    private Product _product;
    private Product _product2;

    public GetBestSellersProducts(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجو دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().Build();
        _product.Category = _category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
        var salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(_product.Id);
        salesInvoice.Count = 2;
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _product2 = new ProductBuilder().WithProductKey("1345").Build();
        _product2.Category = _category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product2));
        var salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(_product2.Id);
        salesInvoice.Count = 1;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '3' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجو دارد")]
    public void AndGiven2()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        var salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(_product.Id);
        salesInvoice.Count = 3;
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven3()
    {
        var salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(_product2.Id);
        salesInvoice.Count = 1;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [When("درخواست مشاهده فهرست کالاهای کم مشتری را میدهم")]
    public void When()
    {
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentFactory =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentFactory, saleInvoiceRepository, unitOfWork);

        _expected = sut.GetBestSellersProducts();
    }

    [Then(
        "باید فهرست کالا ها را به ترتیبفاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000'و فاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' را مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(2);
        _expected.First().Price.Should().Be(_product.Price);
        _expected.First().Brand.Should().Be(_product.Brand);
        _expected.First().Name.Should().Be(_product.Name);
        _expected.First().ProductKey.Should()
            .Be(_product.ProductKey);
        _expected.First().Stock.Should().Be(_product.Stock);
        _expected.First().MaximumAllowableStock.Should()
            .Be(_product.MaximumAllowableStock);
        _expected.First().MinimumAllowableStock.Should()
            .Be(_product.MinimumAllowableStock);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => AndGiven2()
            , _ => AndGiven3()
            , _ => When()
            , _ => Then());
    }
}