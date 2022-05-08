public interface SaleInvoiceService : Service
{
    public void Add(AddSaleInvoiceDto dto);
    public IList<GetSaleInvoiceDto> GetAll();
    public void Update(int id, UpdateSaleInvoiceDto dto);
    public void Delete(int id);
}