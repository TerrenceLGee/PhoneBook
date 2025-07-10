using Microsoft.EntityFrameworkCore;
using PhoneBook.Core.Models;
using PhoneBook.Core.Results;
using PhoneBook.DataAccess.DatabaseOperations;
using PhoneBook.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;


namespace PhoneBook.DataAccess.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly PhonebookContext _context;
    private readonly ILogger<ContactRepository> _logger;
    private string _errorMessage = string.Empty;

    public ContactRepository(PhonebookContext context, ILogger<ContactRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> AddContactAsync(Contact contact)
    {
        try
        {
            if (contact is null)
            {
                _errorMessage = "Contact cannot be null";
                _logger.LogError(_errorMessage);
                return Result.Fail(_errorMessage);
            }
                
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            _errorMessage = $"Database error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
    }

    public async Task<Result> UpdateContactAsync(Contact contact)
    {
        try
        {
            if (contact is null)
            {
                _errorMessage = "Contact cannot be null";
                _logger.LogError(_errorMessage);
                return Result.Fail(_errorMessage);
            }

            var contactToUpdate = await _context.Contacts.FindAsync(contact.Id);

            if (contactToUpdate is null)
            {
                _errorMessage = "Contact not found, unable to update";
                _logger.LogError(_errorMessage);
                return Result.Fail(_errorMessage);
            }

            contactToUpdate.Name = contact.Name;
            contactToUpdate.Email = contact.Email;
            contactToUpdate.PhoneNumber = contact.PhoneNumber;
            contactToUpdate.Address = contact.Address;
            contactToUpdate.Category = contact.Category;

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            _errorMessage = $"Database error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }

    }

    public async Task<Result> DeleteContactAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                _errorMessage = "Id value must be greater than 0";
                _logger.LogError(_errorMessage);
                return Result.Fail(_errorMessage);
            }

            var contactToDelete = await _context.Contacts.FindAsync(id);

            if (contactToDelete is null)
            {
                _errorMessage = $"There was no contact found with id = {id} nothing deleted";
                _logger.LogError(_errorMessage);
                return Result.Fail(_errorMessage);
            }

            _context.Contacts.Remove(contactToDelete);

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            _errorMessage = $"Database error: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result.Fail(_errorMessage);
        }
    }

    public async Task<Result<IReadOnlyList<Contact>>> GetAllContactsAsync()
    {
        try
        {
            var contacts = await _context.Contacts
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Result<IReadOnlyList<Contact>>.Ok(contacts);
        }
        catch (ArgumentNullException ex)
        {
            _errorMessage = $"Method argument cannot be null: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
    }

    public async Task<Result<Contact?>> GetContactByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                _errorMessage = "Id value must be greater than 0";
                _logger.LogError(_errorMessage);
                return Result<Contact?>.Fail(_errorMessage);
            }

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (contact is null)
            {
                _errorMessage = $"No contact with id = {id} found";
                _logger.LogError(_errorMessage);
                return Result<Contact?>.Fail(_errorMessage);
            }

            return Result<Contact?>.Ok(contact);
        }
        catch (ArgumentNullException ex)
        {
            _errorMessage = $"Method argument cannot be null: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
    }

    public async Task<Result<Contact?>> GetContactByNameAsync(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _errorMessage = "Contact name can not be null or blank";
                _logger.LogError(_errorMessage);
                return Result<Contact?>.Fail(_errorMessage);
            }
                
            var contactToFind = await _context.Contacts
                .FirstOrDefaultAsync(c => EF.Functions.Like(c.Name.ToLower(), name.ToLower()));

            if (contactToFind is null)
            {
                _errorMessage = $"No contact with name = {name} found";
                _logger.LogError(_errorMessage);
                return Result<Contact?>.Fail(_errorMessage);
            }
            return Result<Contact?>.Ok(contactToFind);
        }
        catch (ArgumentNullException ex)
        {
            _errorMessage = $"Method argument cannot be null: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<Contact?>.Fail(_errorMessage);
        }
    }

    public async Task<Result<IReadOnlyList<Contact>>> GetContactsByCategoryAsync(string category)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                _errorMessage = "Category name cannot be null or blank";
                _logger.LogError(_errorMessage);
                return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
            }

            var contactsToFind = await _context.Contacts
                .Where(c => EF.Functions.Like(c.Category.ToLower(), category.ToLower()))
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Result<IReadOnlyList<Contact>>.Ok(contactsToFind);
        }
        catch (ArgumentNullException ex)
        {
            _errorMessage = $"Method argument cannot be null: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
        catch (OperationCanceledException ex)
        {
            _errorMessage = $"Operation cancelled: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error has occurred: {ex.Message}";
            _logger.LogError(_errorMessage);
            return Result<IReadOnlyList<Contact>>.Fail(_errorMessage);
        }
    }


}

