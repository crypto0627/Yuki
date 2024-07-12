using Grpc.Core;
using GrpcServices.PaymentService;
using Microsoft.Extensions.Logging;

namespace GrpcServices.Services
{
    public class PaymentService : Payment.PaymentBase
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        public override Task<PaymentReply> MakePayment(PaymentRequest request, ServerCallContext context)
        {
            // 在这里实现您的支付逻辑
            return Task.FromResult(new PaymentReply
            {
                Success = true,
                Message = "Payment made successfully."
            });
        }
    }
}
