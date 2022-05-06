public interface SaleInvoiceRepository : Repository
{
    public void Add(SalesInvoice saleInvoice);
    public IList<GetSaleInvoiceDto> GetAll();
}