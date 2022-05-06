public interface ProductRepository : Repository
{
    public bool IsCategoryContainProduct(int categoryId);
    public void Add(Product product);
    public bool IsProductKeyExistDuringAdd(string productKey);
    public bool IsProductKeyExistDuringUpdate(int id, string productKey);
    public Product Find(int id);
    public void Update(Product product);
    public IList<GetProductDto> GetAll();
    public IList<GetProductDto> GetAvailableProducts();
    public bool IsNumberOfSaleAllowed(int productId, int saleInvoiceCount);
}