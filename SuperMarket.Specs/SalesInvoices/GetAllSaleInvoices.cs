using System.Collections.Generic;
using FluentAssertions;
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
    private IList<GetSaleInvoiceDto> _expected;
    private SalesInvoice _salesInvoice;

    public GetAllSaleInvoices(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "تنها یک فاکتور با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void Given()
    {
        var product = new ProductBuilder().Build();
        product.Category = CategoryFactory.GenerateCategory("نوشیدنی");
        _salesInvoice = SaleInvoiceFactory.GenerateSaleInvoice(1);
        _salesInvoice.Product = product;
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(_salesInvoice));
    }

    [When("درخواست مشاهده فهرست فاکتورها را میدهم")]
    public void When()
    {
        var unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        SaleInvoiceService sut = new SaleInvoiceAppService(
            saleInvoiceRepository,
            productRepository,
            unitOfWork);

        _expected = sut.GetAll();
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
}