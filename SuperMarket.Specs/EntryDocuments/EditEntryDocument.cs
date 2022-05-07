using System.Linq;
using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
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
    private Product _product;
    private UpdateEntryDocumentDto _dto;
    private EntryDocument _entryDocument;

    public EditEntryDocument(ConfigurationFixture configuration) : base(
        configuration)
    {
        _dbContext = CreateDataContext();
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
            EntryDocumentFactory.GenerateEntryDocument(_product.Id);
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' را به سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '30' با قیمت فی '20000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' ویرایش میکنم")]
    public void When()
    {
        _dto = EntryDocumentFactory.GenerateUpdateEntryDocumentDto(
            _product.Id);
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentService sut =
            new EntryDocumentAppService(repository, productRepository,
                unitOfWork);

        sut.Update(_entryDocument.Id, _dto);
    }

    [Then(
        "باید کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '40' در فهرست کالا ها وجود داشته باشد")]
    public void Then()
    {
        var expected = _dbContext.Set<Product>()
            .FirstOrDefault(_ => _.Id == _product.Id);
        expected!.Name.Should().Be(_product.Name);
        expected.Price.Should().Be(_product.Price);
        expected.Stock.Should().Be(_product.Stock);
        expected.Brand.Should().Be(_product.Brand);
        expected.CategoryId.Should().Be(_product.CategoryId);
        expected.ProductKey.Should().Be(_product.ProductKey);
        expected.MinimumAllowableStock.Should()
            .Be(_product.MinimumAllowableStock);
        expected.MaximumAllowableStock.Should()
            .Be(_product.MaximumAllowableStock);
    }

    [And(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '30' با قیمت فی '20000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود داشته باشد")]
    public void AndThen()
    {
        var expected = _dbContext.Set<EntryDocument>().Should().Contain(
            _ => _.Count == _dto.Count && _.ProductId == _dto.ProductId &&
                 _.DateTime == _dto.DateTime &&
                 _.ExpirationDate == _dto.ExpirationDate &&
                 _.ManufactureDate == _dto.ManufactureDate &&
                 _.PurchasePrice == _dto.PurchasePrice);
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