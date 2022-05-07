using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف فاکتور فروش")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فاکتور فروش تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class AddSaleInvoice : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private AddSaleInvoiceDto _dto;

    public AddSaleInvoice(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        _product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '5' با قیمت '25000' را تعریف می کنم")]
    public void When()
    {
        _dto = SaleInvoiceFactory.GenerateAddSaleInvoiceDto(_product.Id);
        var unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        SaleInvoiceService sut = new SaleInvoiceAppService(
            saleInvoiceRepository,
            productRepository,
            unitOfWork);

        sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '5' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        var expected = _dbContext.Set<Product>()
            .FirstOrDefault(_ => _.Id == _product.Id);
        expected!.Stock.Should().Be(_product.Stock);
    }

    [And(
        "باید فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '5' با قیمت '25000' و مجموع قیمت '125000'  در فهرست فاکتورها وجود داشته باشد")]
    public void AndThen()
    {
        var expected = _dbContext.Set<SalesInvoice>().FirstOrDefault();
        expected!.Count.Should().Be(_dto.Count);
        expected.Price.Should().Be(_dto.Price);
        expected.BuyerName.Should().Be(_dto.BuyerName);
        expected.DateTime.Should().Be(_dto.DateTime);
        expected.ProductId.Should().Be(_dto.ProductId);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => When()
            , _ => Then()
            , _ => AndThen());
    }
}