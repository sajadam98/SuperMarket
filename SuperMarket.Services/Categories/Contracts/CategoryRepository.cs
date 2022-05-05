public interface CategoryRepository : Repository
{
    public void Add(Category category);
    public bool IsCategoryNameExistDuringAddCategory(string name);
    public Category Find(int id);
    public void Update(Category category);

    public bool IsCategoryNameExistDuringUpdateCategory(int id,
        string name);

    public void Delete(Category category);
    public IList<GetCategoryDto> GetAll();
}