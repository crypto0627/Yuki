using GrpcServices.Data;
using GrpcServices.Models;
using GrpcServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel server to support HTTP/1.x and HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5047, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AccountDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers(); // 添加控制器支持
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GrpcServices API", Version = "v1" });
});

var app = builder.Build();

// Ensure the database is created and apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(); // 添加 Swagger 中间件
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrpcServices API V1");
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcServices.Services.AccountService>(); // 确保引用正确的类型
    endpoints.MapGrpcService<GrpcServices.Services.ProductService>(); // 确保引用正确的类型
    endpoints.MapGrpcService<GrpcServices.Services.PaymentService>(); // 确保引用正确的类型
    endpoints.MapControllers();  // 确保控制器被映射
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});

app.Run();
