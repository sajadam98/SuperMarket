public class ProductAppService : ProductService
{
    private readonly ProductRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public ProductAppService(ProductRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddProductDto dto)
    {
        var product = new Product
        {
            Brand = dto.Brand,
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            ProductKey = dto.ProductKey,
            MaximumAllowableStock = dto.MaximumAllowableStock,
            MinimumAllowableStock = dto.MinimumAllowableStock,
        };
        _repository.Add(product);
        _unitOfWork.Save();
    }
}