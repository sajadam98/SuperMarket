public interface SaleInvoiceRepository : Repository
{
    public void Add(SalesInvoice saleInvoice);
    public IList<GetSaleInvoiceDto> GetAll();
    public int GetTotalPrice();
    public IList<GetProductSalesReportDto> GetLowCustomerProducts();
    public IList<GetProductSalesReportDto> GetBestSellersProducts();
    public SalesInvoice Find(int id);
    public void Update(SalesInvoice salesInvoice);
    public void Delete(SalesInvoice salesInvoice);
}