public interface EntryDocumentService : Service
{
    public void Add(AddEntryDocumentDto dto);
    public IList<GetEntryDocumentDto> GetAll();
}