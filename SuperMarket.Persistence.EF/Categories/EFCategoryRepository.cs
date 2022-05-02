public class EFCategoryRepository : CategoryRepository
{
    private readonly EFDataContext _dbContext;

    public EFCategoryRepository(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Category category)
    {
        _dbContext.Set<Category>().Add(category);
    }

    public bool IsCategoryNameExist(string name)
    {
        return _dbContext.Set<Category>().Any(_ => _.Name == name);
    }

    public Category Find(int id)
    {
        return _dbContext.Set<Category>().First(_ => _.Id == id);
    }

    public void Update(Category category)
    {
        _dbContext.Set<Category>().Update(category);
    }
}