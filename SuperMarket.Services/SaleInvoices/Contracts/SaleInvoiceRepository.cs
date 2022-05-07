public interface SaleInvoiceRepository : Repository
{
    public void Add(SalesInvoice saleInvoice);
    public IList<GetSaleInvoiceDto> GetAll();
    public int GetTotalPrice();
    public IList<GetProductDto> GetLowCustomerProducts();
    public IList<GetProductDto> GetBestSellersProducts();
    public SalesInvoice Find(int id);
    public void Update(SalesInvoice salesInvoice);
}