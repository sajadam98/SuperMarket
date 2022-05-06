using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف سند ورودی")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود تعریف کنم",
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
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و مجموع قیمت '900000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' را تعریف می کنم")]
    public void When()
    {
        _dto = SaleInvoiceFactory.GenerateAddSaleInvoiceDto(_product.Id);
        var unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        SaleInvoiceService sut = new SaleInvoiceAppService(
            saleInvoiceRepository,
            unitOfWork);

        sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '60' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        var expected = _dbContext.Set<Product>()
            .FirstOrDefault(_ => _.Id == _product.Id);
        expected!.Stock.Should().Be(_dto.Count + _product.Stock);
    }

    [And(
        "سندی با تاریخ '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و مجموع قیمت '900000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' وجود داشته باشد")]
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