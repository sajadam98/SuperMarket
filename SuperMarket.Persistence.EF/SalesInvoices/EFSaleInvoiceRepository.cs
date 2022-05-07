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

    public IList<GetProductDto> GetLowCustomerProducts()
    {
        return _dbContext.Set<SalesInvoice>()
            .OrderBy(_ => _.Count).Select(_ => new GetProductDto
            {
                Brand = _.Product.Brand,
                Id = _.Product.Id,
                Name = _.Product.Name,
                Price = _.Product.Price,
                Stock = _.Product.Stock,
                ProductKey = _.Product.ProductKey,
                MaximumAllowableStock =
                    _.Product.MaximumAllowableStock,
                MinimumAllowableStock = _.Product.MinimumAllowableStock
            }).Distinct().ToList();
    }

    public IList<GetProductDto> GetBestSellersProducts()
    {
        return _dbContext.Set<SalesInvoice>()
            .OrderByDescending(_ => _.Count).Select(_ => new GetProductDto
            {
                Brand = _.Product.Brand,
                Id = _.Product.Id,
                Name = _.Product.Name,
                Price = _.Product.Price,
                Stock = _.Product.Stock,
                ProductKey = _.Product.ProductKey,
                MaximumAllowableStock =
                    _.Product.MaximumAllowableStock,
                MinimumAllowableStock = _.Product.MinimumAllowableStock
            }).Distinct().ToList();
    }
}