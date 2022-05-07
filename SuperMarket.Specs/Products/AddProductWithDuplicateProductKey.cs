using System;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف کالا با کد تکراری")]
[Feature("",
    AsA = "فروشنده ",
    IWantTo = "کالا را تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class
    AddProductWithDuplicateProductKey : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Category _category;
    private Action _expected;

    public AddProductWithDuplicateProductKey(
        ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "دسته بندی با عنوان 'نوشیدنی' وجود دارد")]
    public void Given()
    {
        _category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(_category));
    }

    [And(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' وجود دارد")]
    public void AndGiven()
    {
        var product = new ProductBuilder().WithCategoryId(_category.Id)
            .WithStock(0).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
    }

    [When(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '10' و تعداد موجودی '0' تعریف میکنم")]
    public void When()
    {
        var dto = ProductFactory.GenerateAddProductDto(_category.Id);
        dto.Stock = 0;
        var unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductService sut = new ProductAppService(productRepository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);

        _expected = () => sut.Add(dto);
    }

    [Then(
        "باید خطایی با عنوان 'کد کالا تکراری است'، رخ دهد")]
    public void Then()
    {
        _expected.Should().ThrowExactly<DuplicateProductKeyException>();
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