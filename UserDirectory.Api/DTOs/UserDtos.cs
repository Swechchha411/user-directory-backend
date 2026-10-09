using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Api.DTOs;

public class CreateUserDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 120)]
    public int Age { get; set; }

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required, RegularExpression(@"^[0-9A-Za-z -]{4,10}$",
        ErrorMessage = "Pincode must be 4-10 letters, numbers, spaces, or hyphens.")]
    public string Pincode { get; set; } = string.Empty;
}

public class UpdateUserDto : CreateUserDto
{
}
