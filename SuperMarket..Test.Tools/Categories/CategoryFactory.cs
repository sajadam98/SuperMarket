public class CategoryFactory
{
    public static AddCategoryDto GenerateCategoryDto()
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
}