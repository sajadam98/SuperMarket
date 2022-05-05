
public interface ProductRepository : Repository
{
    public bool IsCategoryContainProduct(int categoryId);
}