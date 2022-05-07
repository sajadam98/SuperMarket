public interface EntryDocumentService : Service
{
    public void Add(AddEntryDocumentDto dto);
    public IList<GetEntryDocumentDto> GetAll();
    public void Update(int id,UpdateEntryDocumentDto dto);
    public void Delete(int id);
}