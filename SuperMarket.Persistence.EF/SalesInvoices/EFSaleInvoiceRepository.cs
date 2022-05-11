public class EFSaleInvoiceRepository : SaleInvoiceRepository
{
    private readonly EFDataContext _dbContext;

    public EFSaleInvoiceRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(SalesInvoice saleInvoice)
    {
        _dbContext.Set<SalesInvoice>().Add(saleInvoice);
    }

    public IList<GetSaleInvoiceDto> GetAll()
    {
        return _dbContext.Set<SalesInvoice>()
            .Select(_ => new GetSaleInvoiceDto
            {
                Count = _.Count,
                Id = _.Id,
                Price = _.Price,
                Product = new GetProductDto
                {
                    Brand = _.Product.Brand,
                    Id = _.Product.Id,
                    Name = _.Product.Name,
                    Price = _.Product.Price,
                    Stock = _.Product.Stock,
                    ProductKey =
                        _.Product.ProductKey,
                    MaximumAllowableStock =
                        _.Product.MaximumAllowableStock,
                    MinimumAllowableStock = _.Product.MinimumAllowableStock
                },
                BuyerName = _.BuyerName,
                DateTime = _.DateTime,
                TotalPrice = _.Count * _.Price
            }).ToList();
    }

    public int GetTotalPrice()
    {
        var totalPrice = 0;
        var saleInvoiceTotalPrice = _dbContext.Set<SalesInvoice>()
            .Select(_ => _.Count * _.Price);
        foreach (var item in saleInvoiceTotalPrice)
        {
            totalPrice += item;
        }

        return totalPrice;
    }

    public IList<GetProductSalesReportDto> GetLowCustomerProducts()
    {
        return _dbContext.Set<Product>()
            .Select(_ => new GetProductSalesReportDto
            {
                Brand = _.Brand,
                Id = _.Id,
                Name = _.Name,
                Price = _.Price,
                Stock = _.Stock,
                ProductKey = _.ProductKey,
                MaximumAllowableStock =
                    _.MaximumAllowableStock,
                MinimumAllowableStock = _.MinimumAllowableStock,
                Count = _.SalesInvoices.Select(_ => _.Count).Sum()
            }).OrderBy(_ => _.Count).ToList();
    }

    public IList<GetProductSalesReportDto> GetBestSellersProducts()
    {
        return _dbContext.Set<Product>()
            .Select(_ => new GetProductSalesReportDto
            {
                Brand = _.Brand,
                Id = _.Id,
                Name = _.Name,
                Price = _.Price,
                Stock = _.Stock,
                ProductKey = _.ProductKey,
                MaximumAllowableStock =
                    _.MaximumAllowableStock,
                MinimumAllowableStock = _.MinimumAllowableStock,
                Count = _.SalesInvoices.Select(_ => _.Count).Sum()
            }).OrderByDescending(_ => _.Count).ToList();
    }

    public SalesInvoice Find(int id)
    {
        return _dbContext.Set<SalesInvoice>()
            .Find(id);
    }

    public void Update(SalesInvoice salesInvoice)
    {
        _dbContext.Set<SalesInvoice>().Update(salesInvoice);
    }

    public void Delete(SalesInvoice salesInvoice)
    {
        _dbContext.Set<SalesInvoice>().Remove(salesInvoice);
    }
}