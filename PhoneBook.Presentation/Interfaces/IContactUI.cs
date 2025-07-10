namespace PhoneBook.Presentation.Interfaces;

public interface IContactUI
{
    Task AddContactAsync();
    Task UpdateContactAsync();
    Task DeleteContactAsync();
    Task ViewContactByIdAsync();
    Task ViewContactByNameAsync();
    Task ViewContactsByCategoryAsync();
    Task SendEmailToContactAsync();
    Task<bool> ViewAllContactsAsync();
}
