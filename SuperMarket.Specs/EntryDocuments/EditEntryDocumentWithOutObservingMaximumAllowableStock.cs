using System;
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
public class
    EditEntryDocumentWithOutObservingMaximumAllowableStock :
        EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private Product _product;
    private EntryDocument _entryDocument;
    private Action _expected;

    public EditEntryDocumentWithOutObservingMaximumAllowableStock(
        ConfigurationFixture configuration) : base(configuration)
    {
        _dbContext = CreateDataContext();
    }

    [Given(
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و حداقل مجاز موجودی '0' و حداکثر موجودی مجاز '20' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _product = new ProductBuilder().WithMaximumAllowableStock(20)
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
        _entryDocument.Count = 10;
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' را به سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '30' با قیمت فی '20000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' ویرایش میکنم")]
    public void When()
    {
        var dto = EntryDocumentFactory.GenerateUpdateEntryDocumentDto(
            _product.Id);
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        EntryDocumentService sut =
            new EntryDocumentAppService(repository, productRepository,
                unitOfWork);

        _expected = () => sut.Update(_entryDocument.Id, dto);
    }

    [Then(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '10' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' در فهرست سندها وجود داشته باشد")]
    public void Then()
    {
        _dbContext.Set<EntryDocument>().Should().Contain(_ =>
            _.Count == _entryDocument.Count &&
            _.ProductId == _entryDocument.ProductId &&
            _.DateTime == _entryDocument.DateTime &&
            _.ExpirationDate == _entryDocument.ExpirationDate &&
            _.ManufactureDate == _entryDocument.ManufactureDate &&
            _.PurchasePrice == _entryDocument.PurchasePrice);
    }

    [And(
        "باید خطایی با عنوان 'سقف مجاز موجودی محصول رعایت نشده است'، رخ دهد")]
    public void AndThen()
    {
        _expected.Should()
            .ThrowExactly<MaximumAllowableStockNotObservedException>();
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