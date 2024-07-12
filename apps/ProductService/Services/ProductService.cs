using Grpc.Core;
using GrpcServices.ProductService;
using Microsoft.Extensions.Logging;

namespace GrpcServices.Services
{
    public class ProductService : Product.ProductBase
    {
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public override Task<ProductReply> GetProduct(ProductRequest request, ServerCallContext context)
        {
            // 在这里实现您的获取产品逻辑
            return Task.FromResult(new ProductReply
            {
                ProductId = 1,
                ProductName = "Sample Product",
                ProductAmount = 10,
                Supplier = "Supplier A",
                ProductDetails = "Details of the product",
                ProductPrice = 100.0,
                IssueDate = "2024-01-01"
            });
        }

        public override Task<CreateProductReply> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            // 在这里实现您的新增产品逻辑
            return Task.FromResult(new CreateProductReply
            {
                Success = true,
                Message = "Product created successfully."
            });
        }
    }
}
