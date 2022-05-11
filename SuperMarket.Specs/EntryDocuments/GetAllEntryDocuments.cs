using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using static BDDHelper;

[Scenario("مشاهده فهرست سندها")]
[Feature("",
    AsA = "فروشنده",
    IWantTo = "فهرست سندها را مشاهده کنم",
    InOrderTo = "فروش کالا را مدیریت کنم"
)]
public class GetAllEntryDocuments : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _dbContext;
    private readonly EntryDocumentAppService _sut;
    private IList<GetEntryDocumentDto> _expected;
    private EntryDocument _entryDocument;

    public GetAllEntryDocuments(ConfigurationFixture configuration) : base(
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
        "تنها یک سند با تاریخ صدور '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و مجموع قیمت '900000' و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' در فهرست سندها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder().Build();
        product.Category = category;
        _entryDocument = new EntryDocumentBuilder().WithCount(50)
            .WithProduct(product).Build();
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(_entryDocument));
    }

    [When("درخواست مشاهده فهرست سندها را میدهم")]
    public void When()
    {
        _expected = _sut.GetAll();
    }

    [Then(
        "درخواست مشاهده فهرست سندها را میدهم.بنابریان: باید سندی با تاریخ صدور '16/04/1900' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و مجموع قیمت '900000' و تاریخ تولید '16/04/1900' و تاریخ انقضا '16/10/1900' در فهرست سندها مشاهده کنم")]
    public void Then()
    {
        _expected.Should().HaveCount(1);
        _expected.Should().Contain(_ =>
            _.Count == _entryDocument.Count &&
            _.Product.Id == _entryDocument.ProductId &&
            _.DateTime == _entryDocument.DateTime &&
            _.ExpirationDate == _entryDocument.ExpirationDate &&
            _.ManufactureDate == _entryDocument.ManufactureDate &&
            _.PurchasePrice == _entryDocument.PurchasePrice);
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