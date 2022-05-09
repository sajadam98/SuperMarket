public class CategoryFactory
{
    public static AddCategoryDto GenerateAddCategoryDto(string name = "لبنیات")
    {
        return new AddCategoryDto
        {
            Name = name
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