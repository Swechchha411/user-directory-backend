
using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Api.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 120,
        ErrorMessage = "Age must be between 1 and 120.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pincode is required.")]
    [RegularExpression(@"^\d{6}$",
        ErrorMessage = "Pincode must contain exactly 6 digits.")]
    public string Pincode { get; set; } = string.Empty;
}

public class UpdateUserDto : CreateUserDto
{
}
