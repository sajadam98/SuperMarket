using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف کالا")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "کالا را تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class AddProduct : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly ProductAppService _sut;
    private AddProductDto _dto;
    private Category _category;

    public AddProduct(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        _sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);
    }

    [Given(
        "دسته بندی با عنوان 'نوشیدنی' وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "هیچ کالایی در فهرست کالا ها وجود ندارد")]
    public void AndGiven()
    {
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' تعریف میکنم")]
    public void When()
    {
        _dto = new AddProductDtoBuilder().WithStock(0).WithName("آب سیب")
            .WithBrand("سن ایچ").WithPrice(25000).WithProductKey("1234")
            .WithMinimumAllowableStock(0).WithMaximumAllowableStock(10)
            .WithCategoryId(_category.Id).Build();

        _sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' در فهرست کالاها وجود داشته باشد")]
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
            , _ => AndGiven()
            , _ => When()
            , _ => Then());
    }
}