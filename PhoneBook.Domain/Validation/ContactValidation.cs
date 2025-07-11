using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PhoneBook.Core.Extensions;
using PhoneBook.Core.Results;
using PhoneNumbers;

namespace PhoneBook.Domain.Validation;

public static class ContactValidation
{
    public static Result ValidateContactData(string name, string phoneNumber, string? email, string? address, string category, ILogger logger, string country)
    {
        var nameResult = ValidateName(name, logger);
        if (!nameResult.IsSuccess) return nameResult;

        var phoneNumberResult = ValidatePhoneNumber(phoneNumber, logger, country);
        if (!phoneNumberResult.IsSuccess) return phoneNumberResult;

        var emailResult = ValidateEmailAddress(email, logger);
        if (!emailResult.IsSuccess) return emailResult;

        var addressResult = ValidateAddress(address, logger);
        if (!addressResult.IsSuccess) return addressResult;

        var categoryResult = ValidateCategory(category, logger);
        if (!categoryResult.IsSuccess) return categoryResult;

        return Result.Ok();
    }

    private static Result ValidatePhoneNumber(string phoneNumber, ILogger logger, string country)
    {
        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumberToValidate = phoneNumberUtil.Parse(phoneNumber, country);
            var isValid = phoneNumberUtil.IsValidNumber(phoneNumberToValidate);

            if (isValid)
                return Result.Ok();

            return logger.LogErrorAndReturnFail(
                $"{phoneNumber} is not a valid phone number for region = {country}");
        }
        catch (NumberParseException ex)
        {
            return logger.LogErrorAndReturnFail($"Number parsing error for {phoneNumber}: {ex.Message}");
        }
    }

    private static Result ValidateCategory(string category, ILogger logger)
    {
        if (!string.IsNullOrWhiteSpace(category))
            return Result.Ok();

        return logger.LogErrorAndReturnFail("Category cannot be null or blank");
    }

    private static Result ValidateName(string name, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(name))
            return logger.LogErrorAndReturnFail("Name cannot be null or blank");

        if (name.Length <= 35 && name.Length >= 1)
            return Result.Ok();

        return logger.LogErrorAndReturnFail(
            $"Provided {name} does not meet the length requirements: greater than or equal to 1 and less than or equal to 25");
    }

    private static Result ValidateEmailAddress(string? emailAddress, ILogger logger)
    {
        if (emailAddress is null)
            return Result.Ok();

        var emailValidationRegex = "^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$";

        if (Regex.IsMatch(emailAddress, emailValidationRegex))
            return Result.Ok();

        return logger.LogErrorAndReturnFail($"{emailAddress} is not a valid email address");
    }

    private static Result ValidateAddress(string? address, ILogger logger)
    {
        if (address is null)
            return Result.Ok();

        if (address.Length > 120)
            return logger.LogErrorAndReturnFail(
                $"The length of the address which is {address.Length} characters is invalid. Address length must be less than 120 characters");

        return Result.Ok();
    }
}