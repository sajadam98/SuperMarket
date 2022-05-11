using System.Linq;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("ویرایش سند ورود")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "سند ورود را ویرایش کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class EditEntryDocument : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly EntryDocumentAppService _sut;
    private Product _product;
    private UpdateEntryDocumentDto _dto;
    private EntryDocument _entryDocument;

    public EditEntryDocument(ConfigurationFixture configuration) : base(
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
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '100' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithMaximumAllowableStock(100)
            .Build();
        _product.Category = category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [And(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '10' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود دارد")]
    public void AndGiven()
    {
        _entryDocument =
            new EntryDocumentBuilder().WithCount(10)
                .WithProductId(_product.Id).Build();
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' را به سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '30' با قیمت فی '20000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' ویرایش میکنم")]
    public void When()
    {
        _dto = new UpdateEntryDocumentDtoBuilder().WithCount(30)
            .WithProductId(_product.Id).Build();
        _sut.Update(_entryDocument.Id, _dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '40' در فهرست کالا ها وجود داشته باشد")]
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
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '30' با قیمت فی '20000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود داشته باشد")]
    public void AndThen()
    {
        var expected = _dbContext.Set<EntryDocument>()
            .FirstOrDefault(_ => _.Id == _entryDocument.Id);
        expected!.Count.Should().Be(_entryDocument.Count);
        expected.DateTime.Should().Be(_entryDocument.DateTime);
        expected.ProductId.Should().Be(_entryDocument.ProductId);
        expected.ExpirationDate.Should().Be(_entryDocument.ExpirationDate);
        expected.ManufactureDate.Should()
            .Be(_entryDocument.ManufactureDate);
        expected.PurchasePrice.Should().Be(_entryDocument.PurchasePrice);
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given()
            , _ => AndGiven()
            , _ => When()
            , _ => Then()
            , _ => AndThen());
    }
}