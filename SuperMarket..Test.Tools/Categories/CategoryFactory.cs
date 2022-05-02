public class CategoryFactory
{
    public static AddCategoryDto GenerateCategoryDto()
    {
        return new AddCategoryDto
        {
            Name = "لبنیات"
        };
    }
}