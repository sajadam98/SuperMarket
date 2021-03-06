public interface CategoryService : Service
{
    public void Add(AddCategoryDto dto);
    public void Update(int id, UpdateCategoryDto dto);
    void Delete(int id);
    public IList<GetCategoryDto> GetAll();
}