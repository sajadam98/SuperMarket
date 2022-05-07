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
        var isNumberOfSaleAllowed =
            _productRepository.IsNumberOfSaleAllowed(dto.ProductId,
                dto.Count);
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
            throw new MinimumAllowableStockNotObservedException();
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
}