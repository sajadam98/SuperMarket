public class ProductAppService : ProductService
{
    private readonly ProductRepository _repository;
    private readonly EntryDocumentRepository _entryDocumentRepository;
    private readonly SaleInvoiceRepository _saleInvoiceRepository;
    private readonly UnitOfWork _unitOfWork;

    public ProductAppService(ProductRepository repository,
        EntryDocumentRepository entryDocumentRepository,
        SaleInvoiceRepository saleInvoiceRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _entryDocumentRepository = entryDocumentRepository;
        _saleInvoiceRepository = saleInvoiceRepository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddProductDto dto)
    {
        var isProductKeyExist =
            _repository.IsProductKeyExist(dto.ProductKey);
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
        var product = _repository.Find(id);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        if (dto.ProductKey != product.ProductKey)
        {
            var isProductKeyExist =
                _repository.IsProductKeyExist(dto.ProductKey);
            if (isProductKeyExist)
            {
                throw new DuplicateProductKeyException();
            }
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

    public int GetProfitAndLossReport()
    {
        var entryDocumentsTotalPurchase =
            _entryDocumentRepository.GetTotalPurchase();
        var saleInvoicesTotalPrice =
            _saleInvoiceRepository.GetTotalPrice();
        return saleInvoicesTotalPrice - entryDocumentsTotalPurchase;
    }

    public IList<GetProductDto> GetLowCustomerProducts()
    {
        return _saleInvoiceRepository.GetLowCustomerProducts();
    }

    public IList<GetProductDto> GetBestSellersProducts()
    {
        return _saleInvoiceRepository.GetBestSellersProducts();
    }
}