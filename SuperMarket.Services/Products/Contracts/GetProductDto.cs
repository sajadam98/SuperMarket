
public class GetProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProductKey { get; set; }
    public int Price { get; set; }
    public string Brand { get; set; }
    public int MinimumAllowableStock { get; set; }
    public int MaximumAllowableStock { get; set; }
    public int Stock { get; set; }
}