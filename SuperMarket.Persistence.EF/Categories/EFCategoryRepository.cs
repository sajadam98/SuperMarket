
public class EFCategoryRepository : CategoryRepository
{
    private readonly EFDataContext _dataContext;

    public EFCategoryRepository(EFDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void Add(Category category)
    {
        _dataContext.Set<Category>().Add(category);
    }
}