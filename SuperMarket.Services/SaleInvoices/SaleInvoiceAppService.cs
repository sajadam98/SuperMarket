public class SaleInvoiceAppService : SaleInvoiceService
{
    private readonly SaleInvoiceRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public SaleInvoiceAppService(SaleInvoiceRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddSaleInvoiceDto dto)
    {
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