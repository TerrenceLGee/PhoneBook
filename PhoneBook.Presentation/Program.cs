using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using PhoneBook.DataAccess.DatabaseOperations;
using PhoneBook.DataAccess.Interfaces;
using PhoneBook.DataAccess.Repositories;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Domain.Services;
using PhoneBook.Presentation.Interfaces;
using PhoneBook.Presentation.UI;
using PhoneBook.Core.Models;

try
{
    await Startup();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return;
}

return;

async Task Startup()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional:true, reloadOnChange: true)
        .Build();

    LoggingSetup();

    var services = new ServiceCollection()
        .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
        .AddDbContext<PhoneBookContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("PhoneBookDb")))
        .Configure<EmailSettings>(configuration.GetSection("EmailSettings"))
        .AddTransient<IContactRepository, ContactRepository>()
        .AddTransient<IContactService, ContactService>()
        .AddTransient<IContactUI, ContactUI>()
        .AddTransient<IEmailService, EmailService>();

    var serviceProvider = services.BuildServiceProvider();

    var contactUI = serviceProvider.GetService<IContactUI>();

    if (contactUI is not null)
    {
        PhoneBookApp phoneBookApp = new PhoneBookApp(contactUI);
        await phoneBookApp.Run();
    }
    else
    {
        throw new Exception("An unexpected error occurred when attempting to run the program");
    }

}

void LoggingSetup()
{
    var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(loggingDirectory);
    var filePath = Path.Combine(loggingDirectory, "app-.txt");
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
        path: filePath,
        rollingInterval: RollingInterval.Day,
        outputTemplate: outputTemplate)
        .CreateLogger();
}
