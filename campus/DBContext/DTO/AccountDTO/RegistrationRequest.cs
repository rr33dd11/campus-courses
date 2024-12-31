using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models.DTO.AccountDTO;

public class RegistrationRequest
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    [Required]
    public DateTime birthDate { get; set; }
    [EmailAddress]
    [Required]
    public string email { get; set; }
    [Required]
    [MinLength(6)]
    [MaxLength(32)]
    public string password { get; set; }
    [Required]
    public string confirmPassword { get; set; }
}