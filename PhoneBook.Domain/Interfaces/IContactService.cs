using PhoneBook.Core.Models;
using PhoneBook.Core.Results;

namespace PhoneBook.Domain.Interfaces;

public interface IContactService
{
    Task<Result> AddContactAsync(string name, string phoneNumber, string? email, string? address, string category);
    Task<Result> UpdateContactAsync(int id, string name, string phoneNumber, string? email, string? address, string category);
    Task<Result> DeleteContactAsync(int id);
    Task<Result<IReadOnlyList<Contact>>> GetAllContactsAsync();
    Task<Result<Contact?>> GetContactByNameAsync(string name);
    Task<Result<Contact?>> GetContactByIdAsync(int id);
    Task<Result<IReadOnlyList<Contact>>> GetContactsByCategoryAsync(string category);
}
