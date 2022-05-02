
public interface CategoryRepository : Repository
{
    public void Add(Category category);
    public bool IsCategoryNameExist(string name);
    public Category Find(int id);
    public void Update(Category category);
}