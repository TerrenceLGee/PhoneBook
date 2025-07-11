using PhoneBook.Domain.Interfaces;
using PhoneBook.Presentation.Interfaces;
using PhoneBook.Presentation.UI.Helpers;
using Spectre.Console;

namespace PhoneBook.Presentation.UI;

public class ContactUI : IContactUI
{
    private readonly IContactService _contactService;
    private readonly IEmailService _emailService;

    public ContactUI(IContactService contactService, IEmailService emailService)
    {
        _contactService = contactService;
        _emailService = emailService;
    }

    public async Task AddContactAsync()
    {
        var name = ContactUIHelper.GetValidInput("Contact name");
        var phoneNumber = ContactUIHelper.GetValidInput("Contact phone number");
        var email = (ContactUIHelper.WantToEnterOptional("Contact email"))
            ? ContactUIHelper.GetValidInput("Contact email")
            : null;
        var address = (ContactUIHelper.WantToEnterOptional("Contact address"))
            ? ContactUIHelper.GetValidInput("Contact address")
            : null;
        var category = ContactUIHelper.GetValidCategory();

        ContactUIHelper.PressAnyKeyToContinue();

        var addContactResult = await _contactService.AddContactAsync(name, phoneNumber, email, address, category);

        if (!addContactResult.IsSuccess)
        {
            ContactUIHelper.DisplayMessage(addContactResult.ErrorMessage!, "red");
        }
        else
        {
            ContactUIHelper.PressAnyKeyToContinue();
            ContactUIHelper.DisplayMessage("Contact added successfully");
            await ViewAllContactsAsync();
        }
    }

    public async Task UpdateContactAsync()
    {
        if (!await ViewAllContactsAsync())
        {
            return;
        }

        int id = ContactUIHelper.GetValidNumber("Id for the contact", "update");

        var contactToBeUpdatedResult = await _contactService.GetContactByIdAsync(id);

        if (!contactToBeUpdatedResult.IsSuccess || contactToBeUpdatedResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{contactToBeUpdatedResult.ErrorMessage}", "red");
            return;
        }

        var contactToUpdate = contactToBeUpdatedResult.Value;

        var name = (ContactUIHelper.WantToEnterOptional("updated contact name"))
            ? ContactUIHelper.GetValidInput("Updated contact name")
            : contactToUpdate.Name;

        var phoneNumber = (ContactUIHelper.WantToEnterOptional("updated contact phone number"))
            ? ContactUIHelper.GetValidInput("Updated contact phone number")
            : contactToUpdate.PhoneNumber;

        var email = (ContactUIHelper.WantToEnterOptional("Updated contact email"))
            ? ContactUIHelper.GetValidInput("Updated contact email")
            : contactToUpdate.Email;

        var address = (ContactUIHelper.WantToEnterOptional("Updated contact address"))
            ? ContactUIHelper.GetValidInput("Updated contact address")
            : contactToUpdate.Address;

        var category = (ContactUIHelper.WantToEnterOptional("Updated contact category"))
            ? ContactUIHelper.GetValidCategory()
            : contactToUpdate.Category;

        var updateContactResult = await _contactService.UpdateContactAsync(id, name, phoneNumber, email, address, category);

        ContactUIHelper.PressAnyKeyToContinue();

        if (!updateContactResult.IsSuccess)
        {
            ContactUIHelper.DisplayMessage($"{updateContactResult.ErrorMessage}", "red");
        }
        else
        {
            ContactUIHelper.PressAnyKeyToContinue();
            ContactUIHelper.DisplayMessage("Contact updated successfully");
            await ViewAllContactsAsync();
        }
    }

    public async Task DeleteContactAsync()
    {
        if (!await ViewAllContactsAsync())
        {
            return;
        }

        int id = ContactUIHelper.GetValidNumber("Id for the contact", "delete");

        var deleteContactResult = await _contactService.DeleteContactAsync(id);

        if (!deleteContactResult.IsSuccess)
        {
            ContactUIHelper.DisplayMessage($"{deleteContactResult.ErrorMessage}", "red");
        }
        else
        {
            ContactUIHelper.PressAnyKeyToContinue();
            ContactUIHelper.DisplayMessage($"Contact with id = {id} successfully deleted");
            await ViewAllContactsAsync();
        }
    }



    public async Task<bool> ViewAllContactsAsync()
    {
        var allContactsResult = await _contactService.GetAllContactsAsync();

        if (!allContactsResult.IsSuccess || allContactsResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{allContactsResult.ErrorMessage}", "red");
            return false;
        }

        var allContacts = allContactsResult.Value;

        if (allContacts.Count == 0)
        {
            ContactUIHelper.DisplayMessage("There are no contacts available to display", "cyan");
            return false;
        }

        var table = new Spectre.Console.Table();
        table.AddColumn("Id");
        table.AddColumn("Category");
        table.AddColumn("Name");
        table.AddColumn("Phone Number");
        table.AddColumn("Email Address");
        table.AddColumn("Home Address");

        foreach (var contact in allContacts)
        {
            table.AddRow(
                contact.Id.ToString(),
                contact.Category,
                contact.Name,
                contact.PhoneNumber,
                contact.Email ?? "N/A",
                contact.Address ?? "N/A");
        }

        AnsiConsole.Write(table);
        return true;
    }

    public async Task ViewContactsByCategoryAsync()
    {
        var category = ContactUIHelper.GetValidCategory();

        var contactsByCategoryResult = await _contactService.GetContactsByCategoryAsync(category);

        if (!contactsByCategoryResult.IsSuccess || contactsByCategoryResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{contactsByCategoryResult.ErrorMessage}", "red");
            return;
        }

        var contactsByCategory = contactsByCategoryResult.Value;

        if (contactsByCategory.Count == 0)
        {
            ContactUIHelper.DisplayMessage($"There are no contacts available to display in the category = {category}", "cyan");
            return;
        }

        var table = new Spectre.Console.Table();

        ContactUIHelper.DisplayMessage($"{category}");

        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Phone Number");
        table.AddColumn("Email Address");
        table.AddColumn("Home Address");

        foreach (var contact in contactsByCategory)
        {
            table.AddRow(
                contact.Id.ToString(),
                contact.Name,
                contact.PhoneNumber,
                contact.Email ?? "N/A",
                contact.Address ?? "N/A");
        }

        AnsiConsole.Write(table);
    }

    public async Task ViewContactByIdAsync()
    {
        if (!await ViewAllContactsAsync())
        {
            return;
        }

        var id = ContactUIHelper.GetValidNumber("id of the contact", "display information for");

        var contactToViewByIdResult = await _contactService.GetContactByIdAsync(id);

        if (!contactToViewByIdResult.IsSuccess || contactToViewByIdResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{contactToViewByIdResult.ErrorMessage}", "red");
            return;
        }

        var contactToViewById = contactToViewByIdResult.Value;

        AnsiConsole.WriteLine($"Information for contact with id = {id}");
        AnsiConsole.WriteLine($"Category: {contactToViewById.Category}");
        AnsiConsole.WriteLine($"Name: {contactToViewById.Name}");
        AnsiConsole.WriteLine($"Phone number: {contactToViewById.PhoneNumber}");
        AnsiConsole.WriteLine($"Email address: {contactToViewById.Email ?? "N/A"}");
        AnsiConsole.WriteLine($"Home address: {contactToViewById.Address ?? "N/A"}");
    }

    public async Task ViewContactByNameAsync()
    {
        if (!await ViewAllContactsAsync())
        {
            return;
        }

        var name = ContactUIHelper.GetValidInput("the name of the contact that you wish to view information for");

        var contactByNameResult = await _contactService.GetContactByNameAsync(name);

        if (!contactByNameResult.IsSuccess || contactByNameResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{contactByNameResult.ErrorMessage}", "red");
            return;
        }

        var contactToViewByName = contactByNameResult.Value;

        AnsiConsole.WriteLine($"Contact information for {name}");
        AnsiConsole.WriteLine($"Category: {contactToViewByName.Category}");
        AnsiConsole.WriteLine($"Phone number: {contactToViewByName.PhoneNumber}");
        AnsiConsole.WriteLine($"Email address: {contactToViewByName.Email ?? "N/A"}");
        AnsiConsole.WriteLine($"Home address: {contactToViewByName.Address ?? "N/A"}");
    }

    public async Task SendEmailToContactAsync()
    {
        if (!await ViewAllContactsAsync())
        {
            return;
        }

        int id = ContactUIHelper.GetValidNumber("id of the user", "send an email to");

        var contactToEmailResult = await _contactService.GetContactByIdAsync(id);

        if (!contactToEmailResult.IsSuccess || contactToEmailResult.Value is null)
        {
            ContactUIHelper.DisplayMessage($"{contactToEmailResult.ErrorMessage}", "red");
            return;
        }

        var contactToEmail = contactToEmailResult.Value;

        if (contactToEmail.Email is null)
        {
            ContactUIHelper.DisplayMessage($"Contact with id = {id} does not have an email address on file", "cyan");
            return;
        }

        ContactUIHelper.PressAnyKeyToContinue();

        var recipientEmailAddress = contactToEmail.Email;
        var recipientName = contactToEmail.Name;

        ContactUIHelper.DisplayMessage($"\nSending email to {recipientName} at email address  {recipientEmailAddress}");

        var subject = ContactUIHelper.WantToEnterOptional("subject of your email")
            ? ContactUIHelper.GetValidInput("Please enter the subject for the email")
            : "(No Subject)";

        var body = ContactUIHelper.GetValidInput("your email message here: ");

        var emailSendingResult = await _emailService.SendEmailAsync(recipientEmailAddress, recipientName, subject, body);

        if (!emailSendingResult.IsSuccess)
        {
            ContactUIHelper.DisplayMessage($"{emailSendingResult.ErrorMessage}", "red");
        }
        else
        {
            ContactUIHelper.DisplayMessage($"Email successfully sent to {recipientEmailAddress}");
        }

    }
}

