using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش کالا")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "کالا را ویرایش کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class EditProduct : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private Category _category;
    private UpdateProductDto _dto;

    public EditProduct(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10'  و تعداد موجودی '0' وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithCategory(_category)
            .WithStock(0).WithMinimumAllowableStock(0)
            .WithMaximumAllowableStock(10).WithName("آب سیب")
            .WithProductKey("1234").WithPrice(25000).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '0' را به کالایی با عنوان 'آب آلبالو' و کدکالا '4321' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و قیمت 25000 و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' ویرایش میکنم")]
    public void When()
    {
        _dto = new UpdateProductDtoBuilder().WithStock(0)
            .WithMinimumAllowableStock(0).WithMaximumAllowableStock(10)
            .WithBrand("سن ایچ").WithName("آب آلبالو").WithPrice(25000)
            .WithCategoryId(_category.Id).WithProductKey("4321").Build();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);

        sut.Update(_product.Id, _dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب آلبالو' و کدکالا '4321' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' وجود داشته باشد")]
    public void Then()
    {
        var expected = _dbContext.Set<Product>().FirstOrDefault();
        expected!.Name.Should().Be(_dto.Name);
        expected.Price.Should().Be(_dto.Price);
        expected.Stock.Should().Be(_dto.Stock);
        expected.CategoryId.Should().Be(_dto.CategoryId);
        expected.ProductKey.Should().Be(_dto.ProductKey);
        expected.MaximumAllowableStock.Should()
            .Be(_dto.MaximumAllowableStock);
        expected.MinimumAllowableStock.Should()
            .Be(_dto.MinimumAllowableStock);
        expected.Brand.Should().Be(_dto.Brand);
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