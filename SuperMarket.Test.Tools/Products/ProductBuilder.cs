public class ProductBuilder
{
    private readonly Product _product = new Product
    {
        Name = "آب سیب",
        ProductKey = "1234",
        Price = 25000,
        Brand = "سن ایچ",
        Stock = 0,
        MaximumAllowableStock = 0,
        MinimumAllowableStock = 0
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

    public ProductBuilder WithCategory(Category category)
    {
        _product.Category = category;
        return this;
    }

    public ProductBuilder WithMaximumAllowableStock(int max)
    {
        _product.MaximumAllowableStock = max;
        return this;
    }
    
    public ProductBuilder WithMinimumAllowableStock(int min)
    {
        _product.MinimumAllowableStock = min;
        return this;
    }
    
    public ProductBuilder WithPrice(int price)
    {
        _product.Price = price;
        return this;
    }

    public Product Build()
    {
        return _product;
    }
}