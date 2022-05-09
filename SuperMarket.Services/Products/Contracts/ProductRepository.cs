public interface ProductRepository : Repository
{
    public bool IsCategoryContainProduct(int categoryId);
    public void Add(Product product);
    public bool IsProductKeyExist(string productKey);
    public Product Find(int id);
    public void Update(Product product);
    public IList<GetProductDto> GetAll();
    public IList<GetProductDto> GetAvailableProducts();
    public bool IsNumberOfSaleAllowed(int productId, int saleInvoiceCount);
    public bool IsMaximumAllowableStockNotObserved(int productId, int count);
}