using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
using Xunit;

public class EntryDocumentServiceTest
{
    private readonly EFDataContext _dbContext;
    private readonly EntryDocumentService _sut;

    public EntryDocumentServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        EntryDocumentRepository repository =
            new EFEntryDocumentRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        _sut = new EntryDocumentAppService(repository, productRepository,
            unitOfWork);
    }

    [Fact]
    public void Add_adds_entry_document_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithMaximumAllowableStock(100)
            .WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto =
            EntryDocumentFactory.GenerateAddEntryDocumentDto(product.Id);

        _sut.Add(dto);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand &&
            _.CategoryId == product.CategoryId &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        _dbContext.Set<EntryDocument>().Should().Contain(_ =>
            _.Count == dto.Count && _.DateTime == dto.DateTime &&
            _.ExpirationDate == dto.ExpirationDate &&
            _.ManufactureDate == dto.ManufactureDate &&
            _.ProductId == dto.ProductId &&
            _.PurchasePrice == dto.PurchasePrice);
    }

    [Fact]
    public void
        Add_throw_MaximumAllowableStockNotObservedException_when_entry_count_plus_stock_is_bigger_than_maximum_allowable_stock()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder()
            .WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto =
            EntryDocumentFactory.GenerateAddEntryDocumentDto(product.Id);

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<MaximumAllowableStockNotObservedException>();
    }

    [Fact]
    public void
        GetAll_returns_entry_documents_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder().Build();
        product.Category = category;
        var entryDocument = EntryDocumentFactory.GenerateEntryDocument();
        entryDocument.Product = product;
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(entryDocument));

        var expected = _sut.GetAll();

        expected.Should().HaveCount(1);
        expected.Should().Contain(_ =>
            _.Count == entryDocument.Count &&
            _.Product.Id == entryDocument.ProductId &&
            _.DateTime == entryDocument.DateTime &&
            _.ExpirationDate == entryDocument.ExpirationDate &&
            _.ManufactureDate == entryDocument.ManufactureDate &&
            _.PurchasePrice == entryDocument.PurchasePrice);
    }
}