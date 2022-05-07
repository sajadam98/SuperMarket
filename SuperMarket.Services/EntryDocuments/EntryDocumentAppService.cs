public class EntryDocumentAppService : EntryDocumentService
{
    private readonly EntryDocumentRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public EntryDocumentAppService(EntryDocumentRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public void Add(AddEntryDocumentDto dto)
    {
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
        _unitOfWork.Save();
    }
}