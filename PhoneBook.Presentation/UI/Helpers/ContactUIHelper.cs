using PhoneBook.Presentation.Options;
using PhoneBook.Presentation.Options.Extensions;
using Spectre.Console;

namespace PhoneBook.Presentation.UI.Helpers;

public static class ContactUIHelper
{
    public static int GetValidNumber(string property1, string property2, string displayColor = "blue")
    {
        return AnsiConsole.Ask<int>($"[{displayColor}]Please enter the {property1} that you wish to {property2} [/]");
    }

    public static string GetValidInput(string property, string displayColor = "blue")
    {
        return AnsiConsole.Ask<string>($"[{displayColor}]Please enter {property} [/]");
    }

    public static bool WantToEnterOptional(string property, string displayColor = "blue")
    {
        return AnsiConsole.Confirm($"[{displayColor}]Do you wish to enter a value for {property}? [/]");
    }

    public static void DisplayMessage(string message, string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }

    public static void PressAnyKeyToContinue(string message = "Press any key to continue", string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    public static MenuOptions DisplayMenuAndGetMenuOption()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
            .Title("Please choose one of the following options")
            .AddChoices(Enum.GetValues<MenuOptions>())
            .UseConverter(choice => choice.GetDisplayName()));
    }

    public static string GetValidCategory()
    {
        var categoryChoice = AnsiConsole.Prompt(
            new SelectionPrompt<CategoryOptions>()
            .Title("Please choose a category for your contact")
            .AddChoices(Enum.GetValues<CategoryOptions>()));

        return categoryChoice.ToString();
    }

   
}

