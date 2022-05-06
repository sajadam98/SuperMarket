using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف سند ورودی بدون رعایت سقف مجاز موجودی")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class
    AddSaleInvoiceWithOutObservingAllowedStock :
        EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private Action _expected;
    private AddSaleInvoiceDto _dto;

    public AddSaleInvoiceWithOutObservingAllowedStock(
        ConfigurationFixture configuration) : base(
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
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        SaleInvoiceService sut = new SaleInvoiceAppService(
            saleInvoiceRepository,
            productRepository,
            unitOfWork);

        _expected = () => sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '60' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        _expected.Should()
            .ThrowExactly<MaximumAllowableProductStockIsNotObserved>();
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => When()
            , _ => Then());
    }
}