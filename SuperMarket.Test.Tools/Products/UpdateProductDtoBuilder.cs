public class UpdateProductDtoBuilder
{
    private UpdateProductDto _dto = new UpdateProductDto
    {
        Name = "آب سیب",
        Price = 25000,
        Brand = "سن ایچ",
        Stock = 0,
        ProductKey = "4321",
        MaximumAllowableStock = 10,
        MinimumAllowableStock = 0,
    };

    public UpdateProductDtoBuilder WithCategoryId(int categoryId)
    {
        _dto.CategoryId = categoryId;
        return this;
    }

    public UpdateProductDtoBuilder WithStock(int stock)
    {
        _dto.Stock = stock;
        return this;
    }

    public UpdateProductDtoBuilder WithMaximumAllowableStock(
        int maximumAllowableStock)
    {
        _dto.MaximumAllowableStock = maximumAllowableStock;
        return this;
    }

    public UpdateProductDtoBuilder WithMinimumAllowableStock(
        int minimumAllowableStock)
    {
        _dto.MinimumAllowableStock = minimumAllowableStock;
        return this;
    }

    public UpdateProductDtoBuilder WithName(
        string name)
    {
        _dto.Name = name;
        return this;
    }

    public UpdateProductDtoBuilder WithPrice(
        int price)
    {
        _dto.Price = price;
        return this;
    }

    public UpdateProductDtoBuilder WithProductKey(
        string productKey)
    {
        _dto.ProductKey = productKey;
        return this;
    }

    public UpdateProductDtoBuilder WithBrand(
        string brand)
    {
        _dto.Brand = brand;
        return this;
    }

    public UpdateProductDto Build()
    {
        return _dto;
    }
}