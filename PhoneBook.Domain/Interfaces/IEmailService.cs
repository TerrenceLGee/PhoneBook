using PhoneBook.Core.Results;

namespace PhoneBook.Domain.Interfaces;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string recipientEmail, string recipientName, string subject, string body);
}
