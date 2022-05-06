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
}