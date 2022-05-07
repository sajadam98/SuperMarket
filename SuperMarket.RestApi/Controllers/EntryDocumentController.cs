using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/entry-documents")]
public class EntryDocumentController
{
    private readonly EntryDocumentService _service;

    public EntryDocumentController(EntryDocumentService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddEntryDocumentDto dto)
    {
        _service.Add(dto);
    }

    [HttpGet]
    public IList<GetEntryDocumentDto> GetAll()
    {
        return _service.GetAll();
    }
}