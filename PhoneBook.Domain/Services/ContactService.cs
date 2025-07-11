using Microsoft.Extensions.Logging;
using PhoneBook.Core.Extensions;
using PhoneBook.Core.Models;
using PhoneBook.Core.Results;
using PhoneBook.DataAccess.Interfaces;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Domain.Validation;
using System.Diagnostics.Metrics;

namespace PhoneBook.Domain.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IContactRepository contactRepository, ILogger<ContactService> logger)
    {
        _contactRepository = contactRepository;
        _logger = logger;
    }

    public async Task<Result> AddContactAsync(string name, string phoneNumber, string? email, string? address, string category, string country)
    {
        var validationResult = ContactValidation.ValidateContactData(name, phoneNumber, email, address, category, _logger, country);

        if (!validationResult.IsSuccess)
            return validationResult;

        var contactToAdd = new Contact
        {
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email,
            Address = address,
            Category = category,
        };

        return await _contactRepository.AddContactAsync(contactToAdd);
    }

    public async Task<Result> UpdateContactAsync(int id, string name, string phoneNumber, string? email, string? address, string category, string country)
    {
        if (id <= 0)
        {
            return _logger.LogErrorAndReturnFail($"{id} is invalid, Ids must be greater than 0");
        }

        var validationResult = ContactValidation.ValidateContactData(name, phoneNumber, email, address, category, _logger, country);

        if (!validationResult.IsSuccess)
            return validationResult;

        var contactToUpdateResult = await _contactRepository.GetContactByIdAsync(id);

        if (!contactToUpdateResult.IsSuccess || contactToUpdateResult.Value is null)
        {
            return _logger.LogErrorAndReturnFail($"There is no contact found with id = {id}");
        }


        var contactToUpdate = contactToUpdateResult.Value;

        contactToUpdate.Name = name;
        contactToUpdate.PhoneNumber = phoneNumber;
        contactToUpdate.Email = email;
        contactToUpdate.Address = address;
        contactToUpdate.Category = category;

        return await _contactRepository.UpdateContactAsync(contactToUpdate);
    }

    public async Task<Result> DeleteContactAsync(int id)
    {
        if (id <= 0)
        {
            return _logger.LogErrorAndReturnFail($"{id} is invalid, Ids must be greater than 0");
        }

        return await _contactRepository.DeleteContactAsync(id);
    }

    public async Task<Result<IReadOnlyList<Contact>>> GetAllContactsAsync()
    {
        return await _contactRepository.GetAllContactsAsync();
    }

    public async Task<Result<Contact?>> GetContactByIdAsync(int id)
    {
        return await _contactRepository.GetContactByIdAsync(id);
    }

    public async Task<Result<Contact?>> GetContactByNameAsync(string name)
    {
        return await _contactRepository.GetContactByNameAsync(name);
    }

    public async Task<Result<IReadOnlyList<Contact>>> GetContactsByCategoryAsync(string category)
    {
        return await _contactRepository.GetContactsByCategoryAsync(category);
    }
}

