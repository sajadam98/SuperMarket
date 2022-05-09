public class AddProductDtoBuilder
{
    private AddProductDto _dto = new AddProductDto
    {
        Name = "آب سیب",
        ProductKey = "1234",
        Price = 25000,
        Brand = "سن ایچ",
        Stock = 0,
        MaximumAllowableStock = 10,
        MinimumAllowableStock = 0
    };

    public AddProductDtoBuilder WithCategoryId(int categoryId)
    {
        _dto.CategoryId = categoryId;
        return this;
    }

    public AddProductDtoBuilder WithStock(int stock)
    {
        _dto.Stock = stock;
        return this;
    }

    public AddProductDtoBuilder WithMaximumAllowableStock(
        int maximumAllowableStock)
    {
        _dto.MaximumAllowableStock = maximumAllowableStock;
        return this;
    }

    public AddProductDtoBuilder WithMinimumAllowableStock(
        int minimumAllowableStock)
    {
        _dto.MinimumAllowableStock = minimumAllowableStock;
        return this;
    }
    
    public AddProductDtoBuilder WithName(
        string name)
    {
        _dto.Name = name;
        return this;
    }
    
    public AddProductDtoBuilder WithPrice(
        int price)
    {
        _dto.Price = price;
        return this;
    }
    
    public AddProductDtoBuilder WithProductKey(
        string productKey)
    {
        _dto.ProductKey = productKey;
        return this;
    }
    
    public AddProductDtoBuilder WithBrand(
        string brand)
    {
        _dto.Brand = brand;
        return this;
    }

    public AddProductDto Build()
    {
        return _dto;
    }
}