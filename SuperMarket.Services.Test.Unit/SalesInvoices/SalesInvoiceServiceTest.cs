using System;
using System.Linq;
using FluentAssertions;
using Xunit;

public class SalesInvoiceServiceTest
{
    private readonly EFDataContext _dbContext;
    private readonly SaleInvoiceService _sut;

    public SalesInvoiceServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository repository =
            new EFSaleInvoiceRepository(_dbContext);
        ProductRepository productRepository =
            new EFProductRepository(_dbContext);
        _sut = new SaleInvoiceAppService(repository, productRepository,
            unitOfWork);
    }

    [Fact]
    public void Add_adds_sale_invoice_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithStock(10)
            .WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto = new AddSalesInvoiceDtoBuilder().WithProductId(product.Id)
            .WithCount(8).Build();

        _sut.Add(dto);

        var expected = _dbContext.Set<Product>()
            .FirstOrDefault(_ => _.Id == product.Id);
        expected!.Stock.Should().Be(product.Stock);
        var expected2 = _dbContext.Set<SalesInvoice>().FirstOrDefault();
        expected2!.Count.Should().Be(dto.Count);
        expected2.Price.Should().Be(dto.Price);
        expected2.BuyerName.Should().Be(dto.BuyerName);
        expected2.DateTime.Should().Be(dto.DateTime);
        expected2.ProductId.Should().Be(dto.ProductId);
    }

    [Fact]
    public void
        Add_throw_AvailableProductStockNotObservedException_when_maximum_allowable_product_inventory_is_not_observed()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder().WithStock(1)
            .WithCategory(category).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto = new AddSalesInvoiceDtoBuilder().WithCount(8)
            .WithProductId(product.Id).Build();

        var expected = () => _sut.Add(dto);

        expected.Should()
            .ThrowExactly<AvailableProductStockNotObservedException>();
    }

    [Fact]
    public void Update_updates_sale_invoice_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        var saleInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(_ => _.Set<SalesInvoice>().Add(saleInvoice));
        var dto = new UpdateSalesInvoiceDtoBuilder()
            .WithProductId(product.Id)
            .Build();

        _sut.Update(saleInvoice.Id, dto);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        var expected = _dbContext.Set<SalesInvoice>()
            .FirstOrDefault(_ => _.Id == saleInvoice.Id);
        expected!.Count.Should().Be(dto.Count);
        expected.BuyerName.Should().Be(dto.BuyerName);
        expected.Price.Should().Be(dto.Price);
        expected.DateTime.Should().Be(dto.DateTime);
        expected.ProductId.Should().Be(dto.ProductId);
    }

    [Fact]
    public void
        Update_throw_AvailableProductStockNotObservedException_when_available_prodcut_stock_not_observed()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .WithStock(10)
            .Build();
        var saleInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(_ => _.Set<SalesInvoice>().Add(saleInvoice));
        var dto =
            new UpdateSalesInvoiceDtoBuilder().WithCount(13)
                .WithProductId(product.Id).Build();

        var expected = () => _sut.Update(saleInvoice.Id, dto);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        expected.Should()
            .ThrowExactly<AvailableProductStockNotObservedException>();
    }

    [Theory]
    [InlineData(-1)]
    public void
        Update_throw_SalesInvoiceNotFoundException_when_sales_invoice_with_given_id_is_not_exist(
            int id)
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto =
            new UpdateSalesInvoiceDtoBuilder().WithProductId(product.Id)
                .Build();

        var expected = () => _sut.Update(id, dto);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        expected.Should()
            .ThrowExactly<SalesInvoiceNotFoundException>();
    }

    [Fact]
    public void Delete_deletes_sale_invoice_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .WithMaximumAllowableStock(20)
            .Build();
        var saleInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(_ => _.Set<SalesInvoice>().Add(saleInvoice));

        _sut.Delete(saleInvoice.Id);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        var expected = _dbContext.Set<SalesInvoice>()
            .Should().NotContain(_ =>
                _.Count == saleInvoice.Count &&
                _.Price == saleInvoice.Price &&
                _.BuyerName == saleInvoice.BuyerName &&
                _.DateTime == saleInvoice.DateTime &&
                _.ProductId == saleInvoice.ProductId);
    }

    [Theory]
    [InlineData(-1)]
    public void
        Delete_throw_SalesInvoiceNotFoundException_sales_invoice_with_given_id_is_not_exist(
            int id)
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        var saleInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(_ => _.Set<SalesInvoice>().Add(saleInvoice));

        var expected = () => _sut.Delete(id);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        expected.Should().ThrowExactly<SalesInvoiceNotFoundException>();
    }

    [Fact]
    public void
        Delete_throw_MaximumAllowableStockNotObservedException_when_maximum_allowable_stock_not_observed()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .WithStock(10)
            .Build();
        var saleInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(_ => _.Set<SalesInvoice>().Add(saleInvoice));

        var expected = () => _sut.Delete(saleInvoice.Id);

        _dbContext.Set<Product>().Should().Contain(_ =>
            _.Brand == product.Brand && _.Id == product.Id &&
            _.Name == product.Name && _.Price == product.Price &&
            _.Stock == product.Stock &&
            _.CategoryId == product.CategoryId &&
            _.ProductKey == product.ProductKey &&
            _.MaximumAllowableStock == product.MaximumAllowableStock &&
            _.MinimumAllowableStock == product.MinimumAllowableStock);
        expected.Should()
            .ThrowExactly<MaximumAllowableStockNotObservedException>();
    }

    [Fact]
    public void GetAll_returns_sales_invoices_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        var product = new ProductBuilder().WithCategory(category).Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var salesInvoice =
            new SalesInvoiceBuilder().WithProduct(product).Build();
        _dbContext.Manipulate(
            _ => _.Set<SalesInvoice>().Add(salesInvoice));

        var expected = _sut.GetAll();

        expected.Should().HaveCount(1);
        expected.Should().Contain(_ =>
            _.Count == salesInvoice.Count &&
            _.Product.Id == salesInvoice.ProductId &&
            _.Price == salesInvoice.Price &&
            _.BuyerName == salesInvoice.BuyerName &&
            _.DateTime == salesInvoice.DateTime && _.TotalPrice ==
            salesInvoice.Count * salesInvoice.Price);
    }
}