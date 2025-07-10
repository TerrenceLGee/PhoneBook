using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Presentation.Options;

public enum MenuOptions
{
    [Display(Name = "Add a contact to the phone book")]
    AddContact,
    [Display(Name = "Update a contact")]
    UpdateContact,
    [Display(Name = "Delete a contact from the phone book")]
    DeleteContact,
    [Display(Name = "Display information for a contact by id")]
    ViewContactById,
    [Display(Name = "Display information for a contact by name")]
    ViewContactByName,
    [Display(Name = "View all contacts in the phone book")]
    ViewAllContacts,
    [Display(Name = "View all contacts in the phone book in the specified category")]
    ViewContactsByCategory,
    [Display (Name = "Send an email to a contact in the phone book")]
    SendEmailToContact,
    [Display(Name = "Exit the program")]
    Exit,
}

