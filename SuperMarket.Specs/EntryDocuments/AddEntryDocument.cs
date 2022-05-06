using System;
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
        "کالایی با عنوان 'آب سیب' و کدکالا '1234' و قیمت '25000' و برند 'سن ایچ' جز دسته بندی 'نوشیدنی' و تعداد موجودی '10' در فهرست کالا ها وجود دارد")]
    public void Given()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        _product = new ProductBuilder().WithMaximumAllowableStock(100).WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(_product));
    }

    [When(
        "سندی با تاریخ صدور '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000' و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' را تعریف می کنم")]
    public void When()
    {
        _dto = new AddEntryDocumentDto
        {
            Count = 50,
            ProductId = _product.Id,
            DateTime = new DateTime(1400, 04, 16),
            ManufactureDate = new DateTime(1400, 04, 16),
            ExpirationDate = new DateTime(1400, 10, 16),
            PurchasePrice = 18000
        };
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        EntryDocumentService sut =
            new EntryDocumentAppService(repository, unitOfWork);
        
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
            _.Stock == _product.Stock + _dto.Count &&
            _.ProductKey == _product.ProductKey &&
            _.MaximumAllowableStock == _product.MaximumAllowableStock &&
            _.MinimumAllowableStock == _product.MinimumAllowableStock);
    }

    [And(
        "سندی با تاریخ '16/04/1400' شامل کالایی با عنوان 'آب سیب' و کدکالا '1234' و تعداد خرید '50' با قیمت فی '18000'و تاریخ تولید '16/04/1400' و تاریخ انقضا '16/10/1400' وجود داشته باشد")]
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