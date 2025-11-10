using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProConnect_Backend.Infrastructure.Data;

public class ProConnectDbContextFactory : IDesignTimeDbContextFactory<ProConnectDbContext>
{
    public ProConnectDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProConnectDbContext>();
        
        // Usar la cadena de conexión de Google Cloud SQL para las migraciones
        var connectionString = "Server=34.95.233.65;Port=3306;Database=Proconnect;Uid=TestDBAcc;Pwd=1Ck28^uHBBIYskjk;SslMode=Required;";
        
        // Usar ServerVersion para MySQL 8.0
        optionsBuilder.UseMySql(
            connectionString, 
            new MySqlServerVersion(new Version(8, 0, 21))
        );

        return new ProConnectDbContext(optionsBuilder.Options);
    }
}

