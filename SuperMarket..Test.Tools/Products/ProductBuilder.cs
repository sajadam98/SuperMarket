public class ProductBuilder
{
    private readonly Product _product = new Product
    {
        Name = "آب سیب",
        ProductKey = "1234",
        Price = 25000,
        Brand = "سن ایچ",
        CategoryId = 1,
        MinimumAllowableStock = 0,
        MaximumAllowableStock = 10,
        Stock = 0
    };

    public ProductBuilder WithProductKey(string productKey)
    {
        _product.ProductKey = productKey;
        return this;
    }

    public ProductBuilder WithCategoryId(int categoryId)
    {
        _product.CategoryId = categoryId;
        return this;
    }

    public Product Build()
    {
        return _product;
    }
}