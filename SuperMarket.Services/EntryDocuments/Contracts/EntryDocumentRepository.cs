public interface EntryDocumentRepository : Repository
{
    public void Add(EntryDocument entryDocument);
    public IList<GetEntryDocumentDto> GetAll();
    public int GetTotalPurchase();
    public void Update(EntryDocument entryDocument);
    public EntryDocument Find(int id);
}