using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductController
{
    private readonly ProductService _service;

    public ProductController(ProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddProductDto dto)
    {
        _service.Add(dto);
    }

    [HttpPut("{id:int}")]
    public void Update(int id, UpdateProductDto dto)
    {
        _service.Update(id, dto);
    }

    [HttpGet]
    public IList<GetProductDto> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("available")]
    public IList<GetProductDto> GetAvailableProducts()
    {
        return _service.GetAll();
    }

    [HttpGet("profit-loss")]
    public int GetProfitAndLossReport()
    {
        return _service.GetProfitAndLossReport();
    }

    [HttpGet("low-customer")]
    public IList<GetProductSalesReportDto> GetLowCustomerProducts()
    {
        return _service.GetLowCustomerProducts();
    }

    [HttpGet("best-seller")]
    public IList<GetProductSalesReportDto> GetBestSellerProducts()
    {
        return _service.GetBestSellersProducts();
    }
}