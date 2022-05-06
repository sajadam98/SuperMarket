public interface SaleInvoiceService : Service
{
    public void Add(AddSaleInvoiceDto dto);
    public IList<GetSaleInvoiceDto> GetAll();
}