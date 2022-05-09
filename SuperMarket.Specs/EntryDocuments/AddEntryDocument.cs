using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("تعریف سند ورود")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود تعریف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class AddEntryDocument : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private AddEntryDocumentDto _dto;

    public AddEntryDocument(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل تعداد موجودی '0' و حداکثر تعداد موجودی '100' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        _product = new ProductBuilder().WithMaximumAllowableStock(100)
            .WithMinimumAllowableStock(0).WithStock(10)
            .WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' را تعریف می کنم")]
    public void When()
    {
        _dto = new AddEntryDocumentDtoBuilder().WithCount(50)
            .WithProductId(_product.Id).Build();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentService sut =
            new EntryDocumentAppService(repository, productRepository,
                unitOfWork);

        sut.Add(_dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '60' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == _product.Brand &&
            _.CategoryId == _product.CategoryId &&
            _.Name == _product.Name && _.Price == _product.Price &&
            _.Stock == _product.Stock &&
            _.ProductKey == _product.ProductKey &&
            _.MaximumAllowableStock == _product.MaximumAllowableStock &&
            _.MinimumAllowableStock == _product.MinimumAllowableStock);
    }

    [And(
        "سندی با تاریخ '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000'و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' وجود داشته باشد")]
    public void AndThen()
    {
        _dbContext.Set<EntryDocument>().Should().Contain(_ =>
            _.Count == _dto.Count && _.DateTime == _dto.DateTime &&
            _.ExpirationDate == _dto.ExpirationDate &&
            _.ManufactureDate == _dto.ManufactureDate &&
            _.ProductId == _dto.ProductId &&
            _.PurchasePrice == _dto.PurchasePrice);
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