public class SaleInvoiceAppService : SaleInvoiceService
{
    private readonly SaleInvoiceRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    private readonly ProductRepository _productRepository;

    public SaleInvoiceAppService(SaleInvoiceRepository repository,
        ProductRepository productRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public void Add(AddSaleInvoiceDto dto)
    {
        var product =
            _productRepository.Find(dto.ProductId);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        var isNumberOfSaleAllowed = product.Stock - dto.Count >= 0;
        if (!isNumberOfSaleAllowed)
        {
            throw new AvailableProductStockNotObservedException();
        }

        var saleInvoice = new SalesInvoice
        {
            Count = dto.Count,
            Price = dto.Price,
            BuyerName = dto.BuyerName,
            DateTime = dto.DateTime,
            ProductId = dto.ProductId
        };
        _repository.Add(saleInvoice);
        saleInvoice.Product.Stock -= dto.Count;
        _unitOfWork.Save();
    }

    public IList<GetSaleInvoiceDto> GetAll()
    {
        return _repository.GetAll();
    }

    public void Update(int id, UpdateSaleInvoiceDto dto)
    {
        var salesInvoice = _repository.Find(id);
        if (salesInvoice == null)
        {
            throw new SalesInvoiceNotFoundException();
        }

        if (dto.Count - salesInvoice.Count >
            salesInvoice.Product.Stock)
        {
            throw new AvailableProductStockNotObservedException();
        }

        salesInvoice.Count = dto.Count;
        salesInvoice.Price = dto.Price;
        salesInvoice.BuyerName = dto.BuyerName;
        salesInvoice.DateTime = dto.DateTime;
        salesInvoice.ProductId = dto.ProductId;

        _repository.Update(salesInvoice);
        salesInvoice.Product.Stock -= dto.Count - salesInvoice.Count;
        _unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var salesInvoice = _repository.Find(id);
        if (salesInvoice == null)
        {
            throw new SalesInvoiceNotFoundException();
        }

        if (salesInvoice.Product.Stock + salesInvoice.Count >
            salesInvoice.Product.MaximumAllowableStock)
        {
            throw new MaximumAllowableStockNotObservedException();
        }

        _repository.Delete(salesInvoice);
        salesInvoice.Product.Stock += salesInvoice.Count;
        _unitOfWork.Save();
    }
}