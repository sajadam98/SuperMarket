using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست کالاهای کم مشتری")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فهرست کالاهای کم مشتری را مشاهده کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetLowCustomerProducts : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private IList<GetProductDto> _expected;
    private Product _product;
    private Product _product2;

    public GetLowCustomerProducts(ConfigurationFixture configuration) :
        base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '10' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithStock(10).WithPrice(25000)
            .WithName("آب سیب").WithProductKey("1234")
            .WithCategory(_category).WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1345' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '10' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void AndGiven()
    {
        _product2 = new ProductBuilder().WithStock(10).WithPrice(25000)
            .WithName("آب سیب").WithProductKey("1345")
            .WithCategory(_category).WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product2));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجو دارد")]
    public void AndGiven2()
    {
        var salesInvoice =
            new SalesInvoiceBuilder().WithCount(2).WithPrice(25000)
                .WithProductId(_product.Id).WithBuyerName("علی علینقیپور")
                .WithDateTime(new DateTime(1900, 04, 16)).Build();
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven3()
    {
        var salesInvoice = new SalesInvoiceBuilder().WithCount(1)
            .WithPrice(25000)
            .WithProductId(_product2.Id).WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16)).Build();
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '3' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجو دارد")]
    public void AndGiven4()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        var salesInvoice = new SalesInvoiceBuilder().WithCount(3)
            .WithPrice(25000).WithProductId(_product.Id)
            .WithBuyerName("علی علینقیپور")
            .WithDateTime(new DateTime(1900, 04, 16)).Build();
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));
    }

    [And(
        "فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000' در فهرست فاکتورها وجود دارد")]
    public void AndGiven5()
    {
        var salesInvoice =
            new SalesInvoiceBuilder().WithCount(1).WithPrice(25000)
                .WithProductId(_product2.Id).WithBuyerName("علی علینقیپور")
                .WithDateTime(new DateTime(1900, 04, 16)).Build();
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

        _expected = sut.GetLowCustomerProducts();
    }

    [Then(
        "باید فهرست کالا ها را به ترتیبفاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'پنیر' و کدکالا '1345' و تعداد خرید '1' با قیمت '25000' و مجموع قیمت '50000'و فاکتوری با تاریخ صدور '16/04/1900' و نام خرید کننده 'علی علینقیپور' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '2' با قیمت '25000' و مجموع قیمت '50000' را مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(2);
        _expected.First().Price.Should().Be(_product2.Price);
        _expected.First().Brand.Should().Be(_product2.Brand);
        _expected.First().Name.Should().Be(_product2.Name);
        _expected.First().ProductKey.Should()
            .Be(_product2.ProductKey);
        _expected.First().Stock.Should().Be(_product2.Stock);
        _expected.First().MaximumAllowableStock.Should()
            .Be(_product2.MaximumAllowableStock);
        _expected.First().MinimumAllowableStock.Should()
            .Be(_product2.MinimumAllowableStock);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => AndGiven2()
            , _ => AndGiven3()
            , _ => AndGiven4()
            , _ => AndGiven5()
            , _ => When()
            , _ => Then());
    }
}