using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.Persistence;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionsBuilder.UseSqlServer();
        
        return new OrderDbContext(optionsBuilder.Options);
    }
}
