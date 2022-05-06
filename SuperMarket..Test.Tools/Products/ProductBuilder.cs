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
        Stock = 10
    };

    public ProductBuilder WithName(string name)
    {
        _product.Name = name;
        return this;
    }

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

    public ProductBuilder WithStock(int stock)
    {
        _product.Stock = stock;
        return this;
    }

    public ProductBuilder WithMaximumAllowableStock(int max)
    {
        _product.MaximumAllowableStock = max;
        return this;
    }

    public Product Build()
    {
        return _product;
    }
}