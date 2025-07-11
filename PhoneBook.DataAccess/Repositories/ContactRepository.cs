using Microsoft.EntityFrameworkCore;
using PhoneBook.Core.Models;
using PhoneBook.Core.Results;
using PhoneBook.DataAccess.DatabaseOperations;
using PhoneBook.DataAccess.Interfaces;
using PhoneBook.Core.Extensions;
using Microsoft.Extensions.Logging;


namespace PhoneBook.DataAccess.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly PhoneBookContext _context;
    private readonly ILogger<ContactRepository> _logger;

    public ContactRepository(PhoneBookContext context, ILogger<ContactRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> AddContactAsync(Contact contact)
    {
        try
        {
            if (contact is null)
                return _logger.LogErrorAndReturnFail("Contact cannot be null");
                
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail($"An unexpected error has occurred: {ex.Message}", ex);
        }
    }

    public async Task<Result> UpdateContactAsync(Contact contact)
    {
        try
        {
            if (contact is null)
                return _logger.LogErrorAndReturnFail("Contact cannot be null");

            var contactToUpdate = await _context.Contacts.FindAsync(contact.Id);

            if (contactToUpdate is null)
                return _logger.LogErrorAndReturnFail("Contact not found, unable to update");

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
            return _logger.LogErrorAndReturnFail($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail($"An unexpected error has occurred: {ex.Message}", ex);
        }

    }

    public async Task<Result> DeleteContactAsync(int id)
    {
        try
        {
            if (id <= 0)
                return _logger.LogErrorAndReturnFail("Id value must be greater than 0");

            var contactToDelete = await _context.Contacts.FindAsync(id);

            if (contactToDelete is null)
                    return _logger.LogErrorAndReturnFail($"There was no contact found with id = {id}"); 

            _context.Contacts.Remove(contactToDelete);

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail($"An unexpected error has occurred: {ex.Message}", ex);
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
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"An unexpected error has occurred: {ex.Message}", ex);
        }
    }

    public async Task<Result<Contact?>> GetContactByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return _logger.LogErrorAndReturnFail<Contact?>("Id value must be greater than 0");

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (contact is null)
                return _logger.LogErrorAndReturnFail<Contact?>($"No contact with id = {id} found");

            return Result<Contact?>.Ok(contact);
        }
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"An unexpected error has occurred: {ex.Message}", ex);
        }
    }

    public async Task<Result<Contact?>> GetContactByNameAsync(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return _logger.LogErrorAndReturnFail<Contact?>("Contact name can not be null or blank");

                
            var contactToFind = await _context.Contacts
                .FirstOrDefaultAsync(c => EF.Functions.Like(c.Name.ToLower(), name.ToLower()));

            if (contactToFind is null)
                return _logger.LogErrorAndReturnFail<Contact?>($"No contact with name = {name} found");

            return Result<Contact?>.Ok(contactToFind);
        }
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"An unexpected error has occurred: {ex.Message}", ex);
        }
    }

    public async Task<Result<IReadOnlyList<Contact>>> GetContactsByCategoryAsync(string category)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(category))
                return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>("Category name cannot be null"); 

            var contactsToFind = await _context.Contacts
                .Where(c => EF.Functions.Like(c.Category.ToLower(), category.ToLower()))
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Result<IReadOnlyList<Contact>>.Ok(contactsToFind);
        }
        catch (DbUpdateException ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"Database error: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"Operation canceled: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            return _logger.LogErrorAndReturnFail<IReadOnlyList<Contact>>($"An unexpected error has occurred: {ex.Message}", ex);
        }
    }
}

