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
        var isNumberOfPurchaseAllowed =
            _productRepository.IsNumberOfPurchaseAllowed(dto.ProductId,
                dto.Count);
        if (!isNumberOfPurchaseAllowed)
        {
            throw new MaximumAllowableProductStockIsNotObserved();
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
        _unitOfWork.Save();
    }
}