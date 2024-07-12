using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using ProductServiceNamespace.Protos;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService.ProductServiceClient _client;

    public ProductsController()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5047"); // 更改為你的 gRPC 服務地址
        _client = new ProductService.ProductServiceClient(channel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var request = new GetProductRequest { Id = id };
        var response = await _client.GetProductAsync(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var response = await _client.CreateProductAsync(request);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
    {
        request.Id = id;
        var response = await _client.UpdateProductAsync(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var request = new DeleteProductRequest { Id = id };
        var response = await _client.DeleteProductAsync(request);
        return Ok(response);
    }
}
