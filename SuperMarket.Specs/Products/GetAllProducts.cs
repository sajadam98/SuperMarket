using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست کالاها")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "فهرست کالاها را مشاهده کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetAllProducts : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private IList<GetProductDto> _expected;

    public GetAllProducts(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '10' وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithCategory(category)
            .WithStock(10).WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).WithName("آب سیب")
            .WithPrice(25000).WithProductKey("1234").Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "درخواست مشاهده فهرست کالاها را میدهم")]
    public void When()
    {
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);

        _expected = sut.GetAll();
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '10' در فهرست کالاها مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(1);
        _expected!.Should().Contain(_ =>
            _.Id == _product.Id && _.Brand == _product.Brand &&
            _.Name == _product.Name && _.Price == _product.Price &&
            _.Stock == _product.Stock &&
            _.ProductKey == _product.ProductKey &&
            _.MaximumAllowableStock == _product.MaximumAllowableStock &&
            _.MinimumAllowableStock == _product.MinimumAllowableStock);
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