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
        var isProductKeyExist =
            _repository.IsProductKeyExistDuringAdd(dto.ProductKey);
        if (isProductKeyExist)
        {
            throw new DuplicateProductKeyException();
        }

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

    public void Update(int id, UpdateProductDto dto)
    {
        var isProductKeyExist =
            _repository.IsProductKeyExistDuringUpdate(id,dto.ProductKey);
        if (isProductKeyExist)
        {
            throw new DuplicateProductKeyException();
        }

        var product = _repository.Find(id);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = dto.Name;
        product.Brand = dto.Brand;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;
        product.ProductKey = dto.ProductKey;
        product.MaximumAllowableStock = dto.MaximumAllowableStock;
        product.MinimumAllowableStock = dto.MinimumAllowableStock;
        
        _repository.Update(product);
        _unitOfWork.Save();
    }

    public IList<GetProductDto> GetAll()
    {
        return _repository.GetAll();
    }

    public IList<GetProductDto> GetAvailableProducts()
    {
        return _repository.GetAvailableProducts();
    }
}