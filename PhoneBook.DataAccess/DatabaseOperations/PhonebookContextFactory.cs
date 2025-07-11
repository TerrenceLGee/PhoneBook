using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PhoneBook.DataAccess.DatabaseOperations;

public class PhoneBookContextFactory : IDesignTimeDbContextFactory<PhoneBookContext>
{
    public PhoneBookContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PhoneBook.Presentation");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PhoneBookContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("PhoneBookDb"));

        return new PhoneBookContext(optionsBuilder.Options);
    }
}

