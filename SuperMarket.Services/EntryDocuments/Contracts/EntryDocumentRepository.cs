public interface EntryDocumentRepository : Repository
{
    public void Add(EntryDocument entryDocument);
    public IList<GetEntryDocumentDto> GetAll();
    public int GetTotalPurchase();
}