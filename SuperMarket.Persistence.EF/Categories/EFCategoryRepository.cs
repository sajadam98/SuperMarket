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

    public bool IsCategoryNameExistDuringAddCategory(string name)
    {
        return _dbContext.Set<Category>().Any(_ => _.Name == name);
    }

    public Category Find(int id)
    {
        return _dbContext.Set<Category>().Find(id);
    }

    public void Update(Category category)
    {
        _dbContext.Set<Category>().Update(category);
    }

    public bool IsCategoryNameExistDuringUpdateCategory(int id,
        string name)
    {
        return _dbContext.Set<Category>().Where(_ => _.Id != id)
            .Any(_ => _.Name == name);
    }

    public void Delete(Category category)
    {
        _dbContext.Set<Category>().Remove(category);
    }

    public IList<GetCategoryDto> GetAll()
    {
        return _dbContext.Set<Category>().Select(_ => new GetCategoryDto
            {
                Id = _.Id,
                Name = _.Name,
            })
            .ToList();
    }
}