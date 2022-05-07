using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست کالاهای کم مشتری")]
[Feature("",
    AsA = "",
    IWantTo = "",
    InOrderTo = ""
)]
public class GetLowCustomerProducts : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private IList<GetProductDto> _expected;
    private SalesInvoice _salesInvoice;

    public GetLowCustomerProducts(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "فاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجو دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder().Build();
        product.Category = _category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(product.Id);
        salesInvoice.Count = 2;
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        var product = new ProductBuilder().WithProductKey("1345").Build();
        product.Category = _category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        _salesInvoice =
            SaleInvoiceFactory.GenerateSaleInvoice(product.Id);
        _salesInvoice.Count = 1;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(_salesInvoice));
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

        _expected = sut.GetLowCustomerProducts();
    }

    [Then(
        "باید فهرست کالا ها را به ترتیبفاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000'و فاکتوری با تاریخ صدور '16/04/1400' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' را مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(2);
        _expected.First().Price.Should().Be(_salesInvoice.Product.Price);
        _expected.First().Brand.Should().Be(_salesInvoice.Product.Brand);
        _expected.First().Name.Should().Be(_salesInvoice.Product.Name);
        _expected.First().ProductKey.Should()
            .Be(_salesInvoice.Product.ProductKey);
        _expected.First().Stock.Should().Be(_salesInvoice.Product.Stock);
        _expected.First().MaximumAllowableStock.Should()
            .Be(_salesInvoice.Product.MaximumAllowableStock);
        _expected.First().MinimumAllowableStock.Should()
            .Be(_salesInvoice.Product.MinimumAllowableStock);
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