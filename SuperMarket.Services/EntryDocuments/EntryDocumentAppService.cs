public class EntryDocumentAppService : EntryDocumentService
{
    private readonly EntryDocumentRepository _repository;
    private readonly ProductRepository _productRepository;
    private readonly UnitOfWork _unitOfWork;

    public EntryDocumentAppService(EntryDocumentRepository repository,
        ProductRepository productRepository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddEntryDocumentDto dto)
    {
        var isMaximumAllowableStockNotObserved =
            _productRepository.IsMaximumAllowableStockNotObserved(
                dto.ProductId,
                dto.Count);
        if (isMaximumAllowableStockNotObserved)
        {
            throw new MaximumAllowableStockNotObservedException();
        }

        var entryDocument = new EntryDocument
        {
            Count = dto.Count,
            DateTime = dto.DateTime,
            ExpirationDate = dto.ExpirationDate,
            ManufactureDate = dto.ManufactureDate,
            ProductId = dto.ProductId,
            PurchasePrice = dto.PurchasePrice
        };
        _repository.Add(entryDocument);
        entryDocument.Product.Stock += dto.Count;
        _unitOfWork.Save();
    }

    public IList<GetEntryDocumentDto> GetAll()
    {
        return _repository.GetAll();
    }

    public void Update(int id, UpdateEntryDocumentDto dto)
    {
        var entryDocument = _repository.Find(id);
        if (entryDocument == null)
        {
            throw new EntryDocumentNotFoundException();
        }

        var isMaximumAllowableStockNotObserved =
            _productRepository.IsMaximumAllowableStockNotObserved(
                dto.ProductId,
                dto.Count - entryDocument.Count);
        if (isMaximumAllowableStockNotObserved)
        {
            throw new MaximumAllowableStockNotObservedException();
        }

        _repository.Update(entryDocument);
        entryDocument.Product.Stock += dto.Count - entryDocument.Count;
        _unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var entryDocument = _repository.Find(id);
        if (entryDocument == null)
        {
            throw new EntryDocumentNotFoundException();
        }

        var isMinimumAllowableStockNotObserved =
            entryDocument.Product.Stock - entryDocument.Count <
            entryDocument.Product.MinimumAllowableStock;
        if (isMinimumAllowableStockNotObserved)
        {
            throw new AvailableProductStockNotObservedException();
        }

        _repository.Delete(entryDocument);
        entryDocument.Product.Stock -= entryDocument.Count;
        _unitOfWork.Save();
    }
}