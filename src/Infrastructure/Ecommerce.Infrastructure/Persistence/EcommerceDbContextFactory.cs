using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ecommerce.Infrastructure.Persistence;

public sealed class EcommerceDbContextFactory : IDesignTimeDbContextFactory<EcommerceDbContext>
{
    public EcommerceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EcommerceDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=ecommerce;Username=postgres");

        return new EcommerceDbContext(optionsBuilder.Options);
    }
}
