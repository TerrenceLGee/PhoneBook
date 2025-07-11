using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Core.Models;

public class Contact
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(25, MinimumLength = 1, ErrorMessage = "Name must be at least 1 character and less than or equal to 25 characters")]
    public string Name { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Phone number must be provided")]
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string Category { get; set; } = string.Empty;
}

