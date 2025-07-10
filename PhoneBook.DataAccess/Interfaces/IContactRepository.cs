using PhoneBook.Core.Models;
using PhoneBook.Core.Results;

namespace PhoneBook.DataAccess.Interfaces;

public interface IContactRepository
{
    Task<Result> AddContactAsync(Contact contact);
    Task<Result> UpdateContactAsync(Contact contact);
    Task<Result> DeleteContactAsync(int id);
    Task<Result<IReadOnlyList<Contact>>> GetAllContactsAsync();
    Task<Result<Contact?>> GetContactByNameAsync(string name);
    Task<Result<Contact?>> GetContactByIdAsync(int id);
    Task<Result<IReadOnlyList<Contact>>> GetContactsByCategoryAsync(string category);
}
