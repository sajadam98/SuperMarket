using System;
using System.Linq;
using FluentAssertions;
using Xunit;

public class SaleInvoiceServiceTest
{
    private readonly EFDataContext _dbContext;
    private readonly SaleInvoiceService _sut;

    public SaleInvoiceServiceTest()
    {
        _dbContext = new EFInMemoryDatabase()
            .CreateDataContext<EFDataContext>();
        UnitOfWork unitOfWork = new EFUnitOfWork(_dbContext);
        SaleInvoiceRepository repository =
            new EFSaleInvoiceRepository(_dbContext);
        _sut = new SaleInvoiceAppService(repository,
            unitOfWork);
    }

    [Fact]
    public void Add_adds_sale_invoice_properly()
    {
        var category = CategoryFactory.GenerateCategory("نوشیدنی");
        _dbContext.Manipulate(_ => _.Set<Category>().Add(category));
        var product = new ProductBuilder().WithCategoryId(category.Id)
            .Build();
        _dbContext.Manipulate(_ => _.Set<Product>().Add(product));
        var dto = SaleInvoiceFactory.GenerateAddSaleInvoiceDto(product.Id);

        _sut.Add(dto);

        var expected = _dbContext.Set<Product>()
            .FirstOrDefault(_ => _.Id == product.Id);
        expected!.Stock.Should().Be(dto.Count + product.Stock);
        var expected2 = _dbContext.Set<SalesInvoice>().FirstOrDefault();
        expected2!.Count.Should().Be(dto.Count);
        expected2.Price.Should().Be(dto.Price);
        expected2.BuyerName.Should().Be(dto.BuyerName);
        expected2.DateTime.Should().Be(dto.DateTime);
        expected2.ProductId.Should().Be(dto.ProductId);
    }
}