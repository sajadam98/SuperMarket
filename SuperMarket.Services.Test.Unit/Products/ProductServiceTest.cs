using System.Linq;
using FluentAssertions;
using SuperMarket._Test.Tools.EntryDocuments;
using Xunit;

public class ProductServiceTest
{
    private readonly EFDataContext _dbContext;
    private readonly ProductService _sut;

    public ProductServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        ProductRepository repository =
            new EFProductRepository(_dbContext);
        EntryDocumentRepository entryDocumentRepository =
            new EFEntryDocumentRepository(_dbContext);
        SaleInvoiceRepository saleInvoiceRepository =
            new EFSaleInvoiceRepository(_dbContext);
        _sut = new ProductAppService(repository,
            entryDocumentRepository, saleInvoiceRepository, unitOfWork);
    }

    [Fact]
    public void Add_adds_product_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var dto = ProductFactory.GenerateAddProductDto(category.Id);

        _sut.Add(dto);

        var expected = _dbContext.Set<Product>().FirstOrDefault();
        expected!.Name.Should().Be(dto.Name);
        expected.Price.Should().Be(dto.Price);
        expected.Stock.Should().Be(dto.Stock);
        expected.CategoryId.Should().Be(dto.CategoryId);
        expected.ProductKey.Should().Be(dto.ProductKey);
        expected.MaximumAllowableStock.Should()
            .Be(dto.MaximumAllowableStock);
        expected.MinimumAllowableStock.Should()
            .Be(dto.MinimumAllowableStock);
        expected.Brand.Should().Be(dto.Brand);
    }

    [Fact]
    public void
        Add_throw_DuplicateProductKeyException_with_duplicated_product_key()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto = ProductFactory.GenerateAddProductDto(category.Id);

        var expected = () => _sut.Add(dto);

        expected.Should().ThrowExactly<DuplicateProductKeyException>();
    }

    [Fact]
    public void
        Update_updates_product_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto =
            ProductFactory.GenerateUpdateProductDto(category.Id, "4321");

        _sut.Update(product.Id, dto);

        var expected = _dbContext.Set<Product>().FirstOrDefault();
        expected!.Name.Should().Be(dto.Name);
        expected.Price.Should().Be(dto.Price);
        expected.Stock.Should().Be(dto.Stock);
        expected.CategoryId.Should().Be(dto.CategoryId);
        expected.ProductKey.Should().Be(dto.ProductKey);
        expected.MaximumAllowableStock.Should()
            .Be(dto.MaximumAllowableStock);
        expected.MinimumAllowableStock.Should()
            .Be(dto.MinimumAllowableStock);
        expected.Brand.Should().Be(dto.Brand);
    }

    [Theory]
    [InlineData(-1)]
    public void
        Update_throw_ProductNotFoundException_with_given_id(int id)
    {
        var dto =
            ProductFactory.GenerateUpdateProductDto(id);

        var expected = () => _sut.Update(id, dto);

        expected.Should().ThrowExactly<ProductNotFoundException>();
    }

    [Fact]
    public void
        Update_throw_DuplicateProductKeyException_when_product_key_is_exist()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var product2 = new ProductBuilder().WithCategoryId(category.Id)
            .WithProductKey("4321").WithName("آب انبه")
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product2));
        var dto =
            ProductFactory.GenerateUpdateProductDto(category.Id, "4321");

        var expected = () => _sut.Update(product.Id, dto);

        expected.Should().ThrowExactly<DuplicateProductKeyException>();
    }

    [Fact]
    public void GetAll_returns_products_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));

        var expected = _sut.GetAll();

        expected.Should().HaveCount(1);
        expected!.Should().Contain(_ =>
            _.Id == product.Id && _.Brand == product.Brand &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
    }

    [Fact]
    public void
        GetAvailableProducts_returns_products_that_available_in_super_market_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var product2 = new ProductBuilder().WithCategoryId(category.Id)
            .WithProductKey("4321").WithName("آب انبه").WithStock(0)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product2));

        var expected = _sut.GetAvailableProducts();

        expected.Should().HaveCount(1);
        expected!.Should().Contain(_ =>
            _.Id == product.Id && _.Brand == product.Brand &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
    }
    
    [Fact]
    public void
        GetProfitAndLossReport_returns_profit_or_loss_report()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder()
            .Build();
        product.Category = category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var saleInvoice = SaleInvoiceFactory.GenerateSaleInvoice();
        saleInvoice.Product = product;
        saleInvoice.Count = 2;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(saleInvoice));
        var entryDocument =
            EntryDocumentFactory.GenerateEntryDocument(product.Id);
        _dbContext.Manipulate(_ =>
            _.Set<EntryDocument>().Add(entryDocument));

        var expected = _sut.GetProfitAndLossReport();

        expected.Should().Be(saleInvoice.Count * saleInvoice.Price -
                             entryDocument.Count *
                             entryDocument.PurchasePrice);
    }
    
    [Fact]
    public void
        GetLowCustomerProducts_returns_products_that_less_sold_than_the_rest()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder()
            .Build();
        product.Category = category;
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var salesInvoice = SaleInvoiceFactory.GenerateSaleInvoice();
        salesInvoice.Product = product;
        salesInvoice.Count = 2;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(salesInvoice));
        var salesInvoice2 = SaleInvoiceFactory.GenerateSaleInvoice();
        salesInvoice2.Product = product;
        salesInvoice2.Count = 1;
        _dbContext.Manipulate(_ =>
            _.Set<SalesInvoice>().Add(salesInvoice2));

        var expected = _sut.GetLowCustomerProducts();

        expected.Should().HaveCount(2);
        expected.First().Price.Should().Be(salesInvoice2.Price);
    }
}