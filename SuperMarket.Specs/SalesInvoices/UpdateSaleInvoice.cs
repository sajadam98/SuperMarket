using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش فاکتور فروش")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فاکتور فروش تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class UpdateSaleInvoice : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private SalesInvoice _salesInvoice;
    private UpdateSaleInvoiceDto _dto;

    public UpdateSaleInvoice(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().Build();
        _product.Category = category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven()
    {
        _salesInvoice = SaleInvoiceFactory.GenerateSaleInvoice();
        _salesInvoice.Product = _product;
        _salesInvoice.Count = 2;
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(_salesInvoice));
    }

    [When(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' را به فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '8' با قیمت '24000' ویرایش می کنم")]
    public void When()
    {
        _dto = SaleInvoiceFactory.GenerateUpdatealeInvoiceDto(_product.Id);
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository repository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        SaleInvoiceService sut =
            new SaleInvoiceAppService(repository, productRepository,
                unitOfWork);

        sut.Update(_salesInvoice.Id, _dto);
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
        var expected = _dbContext.Set<SalesInvoice>().FirstOrDefault(_ => _.Id==_salesInvoice.Id);
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