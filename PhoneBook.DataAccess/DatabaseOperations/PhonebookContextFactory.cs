using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PhoneBook.DataAccess.DatabaseOperations;

public class PhonebookContextFactory : IDesignTimeDbContextFactory<PhonebookContext>
{
    public PhonebookContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PhoneBook.Presentation");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PhonebookContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("PhoneBookDb"));

        return new PhonebookContext(optionsBuilder.Options);
    }
}

