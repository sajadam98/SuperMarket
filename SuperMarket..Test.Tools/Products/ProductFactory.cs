public class ProductFactory
{
    public static AddProductDto GenerateAddProductDto(int categoryId) =>
        new AddProductDto
        {
            Name = "آب سیب",
            ProductKey = "1234",
            Price = 25000,
            Brand = "سن ایچ",
            CategoryId = categoryId,
            MinimumAllowableStock = 0,
            MaximumAllowableStock = 10,
            Stock = 0
        };

    public static UpdateProductDto
        GenerateUpdateProductDto(int categoryId, string productKey = "1234") => new UpdateProductDto
    {
        Name = "آب سیب",
        ProductKey = productKey,
        Price = 25000,
        Brand = "سن ایچ",
        CategoryId = categoryId,
        MinimumAllowableStock = 0,
        MaximumAllowableStock = 10,
        Stock = 0
    };
}