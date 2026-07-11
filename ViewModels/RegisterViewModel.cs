using System.ComponentModel.DataAnnotations;

namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$", ErrorMessage = "Password must contain uppercase, lowercase, number and special character.")]
    public string Password { get; set; } = "";

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = "";
}