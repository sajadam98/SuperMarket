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

    public bool IsProductKeyExist(string productKey)
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
        return _dbContext.Set<Product>().Find(id);
    }

    public void Update(Product product)
    {
        _dbContext.Set<Product>().Update(product);
    }

    public IList<GetProductDto> GetAll()
    {
        return _dbContext.Set<Product>().Select(_ => new GetProductDto
        {
            Brand = _.Brand,
            Id = _.Id,
            Name = _.Name,
            Price = _.Price,
            Stock = _.Stock,
            ProductKey = _.ProductKey,
            MaximumAllowableStock = _.MaximumAllowableStock,
            MinimumAllowableStock = _.MinimumAllowableStock
        }).ToList();
    }

    public IList<GetProductDto> GetAvailableProducts()
    {
        return _dbContext.Set<Product>().Where(_ => _.Stock > 0).Select(
            _ => new GetProductDto
            {
                Brand = _.Brand,
                Id = _.Id,
                Name = _.Name,
                Price = _.Price,
                Stock = _.Stock,
                ProductKey = _.ProductKey,
                MaximumAllowableStock = _.MaximumAllowableStock,
                MinimumAllowableStock = _.MinimumAllowableStock
            }).ToList();
    }

    public bool IsMaximumAllowableStockNotObserved(int productId,
        int count)
    {
        var product = _dbContext.Set<Product>()
            .Single(_ => _.Id == productId);
        return product.Stock + count > product.MaximumAllowableStock;
    }
}