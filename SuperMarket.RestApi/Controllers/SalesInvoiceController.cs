using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/sales-invoices")]
public class SalesInvoiceController
{
    private readonly SaleInvoiceService _service;

    public SalesInvoiceController(SaleInvoiceService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddSaleInvoiceDto dto)
    {
        _service.Add(dto);
    }

    [HttpGet]
    public IList<GetSaleInvoiceDto> GetAll()
    {
        return _service.GetAll();
    }
}