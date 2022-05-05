
public class AddProductDto
{
    public string Name { get; set; }
    public string ProductKey { get; set; }
    public int Price { get; set; }
    public string Brand { get; set; }
    public int MinimumAllowableStock { get; set; }
    public int MaximumAllowableStock { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
}