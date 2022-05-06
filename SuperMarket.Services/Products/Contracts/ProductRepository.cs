
public interface ProductRepository : Repository
{
    public bool IsCategoryContainProduct(int categoryId);
    public void Add(Product product);
    public bool IsProductKeyExist(string productKey);
}