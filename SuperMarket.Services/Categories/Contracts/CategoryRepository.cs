
public interface CategoryRepository : Repository
{
    public void Add(Category category);
    public bool IsCategoryNameExist(string name);
}