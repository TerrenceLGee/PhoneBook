using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Presentation.Options;

public enum CategoryOptions
{
    [Display (Name = "General")]
    General,
    [Display(Name = "Family")]
    Family,
    [Display(Name = "Friend")]
    Friend,
    [Display(Name = "Work")]
    Work,
}

