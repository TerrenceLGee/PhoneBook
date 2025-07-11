using PhoneBook.Core.Models;
using PhoneBook.Core.Results;
using PhoneBook.DataAccess.Interfaces;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Core.Extensions;
using PhoneNumbers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

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

    public async Task<Result> AddContactAsync(string name, string phoneNumber, string? email, string? address, string category)
    {
        var validationResult = ValidateContactData(name, email, phoneNumber, address, category);

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

    public async Task<Result> UpdateContactAsync(int id, string name, string phoneNumber, string? email, string? address, string category)
    {
        if (id <= 0)
        {
            return _logger.LogErrorAndReturnFail($"{id} is invalid, Ids must be greater than 0");
        }
            
        var validationResult = ValidateContactData(name, email, phoneNumber, address, category);

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
        if (id <= 0)
        {
            return _logger.LogErrorAndReturnFail<Contact?>($"{id} is invalid, Ids must be greater than 0");
        }
            

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

    private Result ValidatePhoneNumber(string phoneNumber, string country = "US")
    {
        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumberToValidate = phoneNumberUtil.Parse(phoneNumber, country);
            var isValid = phoneNumberUtil.IsValidNumber(phoneNumberToValidate);

            if (isValid)
            {
                return Result.Ok();
            }
            else
            {
                return _logger.LogErrorAndReturnFail($"{phoneNumber} is not a valid phone number for region = {country}");
            }
        }
        catch (NumberParseException ex)
        {
            return _logger.LogErrorAndReturnFail($"Number parsing error for {phoneNumber}: {ex.Message}");
        }
    }

    private Result ValidateCategory(string category)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            return Result.Ok();
        }
        else
        {
            return _logger.LogErrorAndReturnFail("Category cannot be null or blank");
        }
    }
    
    private Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return _logger.LogErrorAndReturnFail("Name cannot be null or blank");
        }
        else if (name.Length <= 25 && name.Length >= 1)
        {
            return Result.Ok();
        }
        else
        {
            return _logger.LogErrorAndReturnFail($"Provided {name} does not meet the length requirements: greater than or equal to 1 and less than or equal to 25");
        }
    }

    private Result ValidateEmailAddress(string? emailAddress)
    {
        if (emailAddress is null)
            return Result.Ok();

        var emailValidationRegex = "^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$";

        if (Regex.IsMatch(emailAddress, emailValidationRegex))
        {
            return Result.Ok();
        }
        else
        {
            return _logger.LogErrorAndReturnFail($"{emailAddress} is not a valid email address");
        }
    }

    private Result ValidateAddress(string? address)
    {
        if (address is null)
        {
            return Result.Ok();
        }
        if (address.Length > 120)
        {
            return _logger.LogErrorAndReturnFail($"The length of the address is invalid must be less than 120 characters");
        }
        return Result.Ok();
    }

    private Result ValidateContactData(string name, string? email, string phoneNumber, string? address, string category)
    {
        var nameResult = ValidateName(name);
        if (!nameResult.IsSuccess) return nameResult;

        var emailResult = ValidateEmailAddress(email);
        if (!emailResult.IsSuccess) return emailResult;

        var phoneNumberResult = ValidatePhoneNumber(phoneNumber);
        if (!phoneNumberResult.IsSuccess) return phoneNumberResult;

        var addressResult = ValidateAddress(address!);
        if (!addressResult.IsSuccess) return addressResult;

        var categoryResult = ValidateCategory(category);
        if (!categoryResult.IsSuccess) return categoryResult;

        return Result.Ok();
    }
}

