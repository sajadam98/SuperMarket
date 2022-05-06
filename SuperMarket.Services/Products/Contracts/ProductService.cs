
public interface ProductService : Service
{
    public void Add(AddProductDto dto);
    public void Update(int productId, UpdateProductDto dto);
}