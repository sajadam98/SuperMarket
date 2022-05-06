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
}