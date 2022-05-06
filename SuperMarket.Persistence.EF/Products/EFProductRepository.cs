public class EFProductRepository : ProductRepository
{
    private readonly EFDataContext _dbContext;

    public EFProductRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool IsCategoryContainProduct(int categoryId)
    {
        return _dbContext.Set<Product>()
            .Any(_ => _.CategoryId == categoryId);
    }

    public void Add(Product product)
    {
        _dbContext.Set<Product>().Add(product);
    }

    public bool IsProductKeyExistDuringAdd(string productKey)
    {
        return _dbContext.Set<Product>()
            .Any(_ => _.ProductKey == productKey);
    }

    public bool IsProductKeyExistDuringUpdate(int id, string productKey)
    {
        return _dbContext.Set<Product>().Where(_ => _.Id != id)
            .Any(_ => _.ProductKey == productKey);
    }

    public Product Find(int id)
    {
        return _dbContext.Set<Product>().FirstOrDefault(_ => _.Id == id);
    }

    public void Update(Product product)
    {
        _dbContext.Set<Product>().Update(product);
    }
}