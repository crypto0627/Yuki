using Microsoft.OpenApi.Models;
using GrpcServices.AccountService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure gRPC client
builder.Services.AddGrpcClient<GrpcServices.AccountService.Account.AccountClient>(o =>
{
    o.Address = new Uri("http://localhost:5047"); // 修改为 gRPC 服务的地址
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();