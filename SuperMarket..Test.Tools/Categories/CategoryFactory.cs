public class CategoryFactory
{
    public static AddCategoryDto GenerateAddCategoryDto()
    {
        return new AddCategoryDto
        {
            Name = "لبنیات"
        };
    }
    
    public static Category GenerateCategory(string name = "لبنیات")
    {
        return new Category
        {
            Name = name
        };
    }
    
    public static UpdateCategoryDto GenerateUpdateCategoryDto(string name = "خشکبار")
    {
        return new UpdateCategoryDto
        {
            Name = name
        };
    }
}