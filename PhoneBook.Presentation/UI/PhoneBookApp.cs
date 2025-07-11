using PhoneBook.Presentation.Interfaces;
using PhoneBook.Presentation.UI.Helpers;
using PhoneBook.Presentation.Options;

namespace PhoneBook.Presentation.UI;

public class PhoneBookApp
{
    private readonly IContactUI _contactUI;

    public PhoneBookApp(IContactUI contactUI)
    {
        _contactUI = contactUI;
    }

    public async Task Run()
    {
        bool isRunning = true;

        ContactUIHelper.DisplayMessage("Welcome to the phone book app\n");

        while (isRunning)
        {
            switch (ContactUIHelper.DisplayMenuAndGetMenuOption())
            {
                case MenuOptions.AddContact:
                    await _contactUI.AddContactAsync();
                    break;
                case MenuOptions.UpdateContact:
                    await _contactUI.UpdateContactAsync();
                    break;
                case MenuOptions.DeleteContact:
                    await _contactUI.DeleteContactAsync();
                    break;
                case MenuOptions.ViewContactById:
                    await _contactUI.ViewContactByIdAsync();
                    break;
                case MenuOptions.ViewContactByName:
                    await _contactUI.ViewContactByNameAsync();
                    break;
                case MenuOptions.ViewContactsByCategory:
                    await _contactUI.ViewContactsByCategoryAsync();
                    break;
                case MenuOptions.ViewAllContacts:
                    await _contactUI.ViewAllContactsAsync();
                    ContactUIHelper.PressAnyKeyToContinue("Press any key to return to the main menu");
                    break;
                case MenuOptions.SendEmailToContact:
                    await _contactUI.SendEmailToContactAsync();
                    break;
                case MenuOptions.Exit:
                    isRunning = false;
                    break;
            }
        }
    }
}

