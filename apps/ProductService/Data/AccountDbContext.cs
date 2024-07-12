using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrpcServices.Models;

namespace GrpcServices.Data
{
    public class AccountDbContext : IdentityDbContext<User>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }

        // 其他 DbSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
