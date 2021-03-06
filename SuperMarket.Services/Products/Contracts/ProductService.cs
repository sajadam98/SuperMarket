public interface ProductService : Service
{
    public void Add(AddProductDto dto);
    public void Update(int productId, UpdateProductDto dto);
    public IList<GetProductDto> GetAll();
    public IList<GetProductDto> GetAvailableProducts();
    public int GetProfitAndLossReport();
    public IList<GetProductSalesReportDto> GetLowCustomerProducts();
    public IList<GetProductSalesReportDto> GetBestSellersProducts();
}