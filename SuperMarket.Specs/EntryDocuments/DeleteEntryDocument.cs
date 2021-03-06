using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("حذف سند ورود")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود را حذف کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class DeleteEntryDocument : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly EntryDocumentAppService _sut;
    private EntryDocument _entryDocument;
    private Product _product;

    public DeleteEntryDocument(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        _sut =
            new EntryDocumentAppService(repository, productRepository,
                unitOfWork);
    }

    [Given(
        "الایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '100' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithMaximumAllowableStock(100)
            .WithStock(10).WithMinimumAllowableStock(0)
            .Build();
        _product.Category = category;
        _entryDocument = new EntryDocumentBuilder().WithCount(10)
            .WithProduct(_product).Build();
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '10' با قیمت فی '18000' و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' را حذف میکنم")]
    public void When()
    {
        _sut.Delete(_entryDocument.Id);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '10' در فهرست کالا ها وجود داشته باشد")]
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
        "نباید سندی با تاریخ صدور '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '10' با قیمت فی '18000' و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' در فهرست سندها وجود داشته باشد")]
    public void AndThen()
    {
        _dbContext.Set<EntryDocument>().Should().NotContain(_ =>
            _.Count == _entryDocument.Count &&
            _.DateTime == _entryDocument.DateTime &&
            _.ExpirationDate == _entryDocument.ExpirationDate &&
            _.ManufactureDate == _entryDocument.ManufactureDate &&
            _.ProductId == _entryDocument.ProductId &&
            _.PurchasePrice == _entryDocument.PurchasePrice);
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