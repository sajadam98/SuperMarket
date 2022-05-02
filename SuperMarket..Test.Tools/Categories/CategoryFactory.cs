public class CategoryFactory
{
    public static AddCategoryDto GenerateAddCategoryDto()
    {
        return new AddCategoryDto
        {
            Name = "لبنیات"
        };
    }
    
    public static Category GenerateCategory()
    {
        return new Category
        {
            Name = "لبنیات"
        };
    }
    
    public static UpdateCategoryDto GenerateUpdateCategoryDto()
    {
        return new UpdateCategoryDto
        {
            Name = "خشکبار"
        };
    }
}