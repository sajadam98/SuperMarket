public class EFProductRepository : ProductRepository
{
    private readonly EFDataContext _dbContext;

    public EFProductRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsCategoryContainProduct(int categoryId)
    {
        return _dbContext.Set<Product>().Any(_ => _.CategoryId == categoryId);
    }

    public void Add(Product product)
    {
        _dbContext.Set<Product>().Add(product);
    }
}