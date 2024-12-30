using Microsoft.EntityFrameworkCore;

namespace ProductApi.Infrastructure.Data;

using Domain.Entity;
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
}
