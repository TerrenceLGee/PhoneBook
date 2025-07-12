using Serilog;

namespace PhoneBook.Presentation.Options.Extensions;

public static class ExceptionExtensions
{
    public static void LogAndDisplayFatalError(this Exception exception, string message)
    {
        Console.WriteLine($"FATAL ERROR: {message}");
        Console.WriteLine($"Exception: {exception.Message}");
        Log.Fatal(exception, message);
    }
}

