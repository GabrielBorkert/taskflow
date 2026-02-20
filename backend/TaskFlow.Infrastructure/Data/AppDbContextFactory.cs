using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-6MQT45P\\MSSQLSERVERDEV;Database=TaskFlow;Trusted_Connection=true;TrustServerCertificate=true;");

        return new AppDbContext(optionsBuilder.Options);
    }
}