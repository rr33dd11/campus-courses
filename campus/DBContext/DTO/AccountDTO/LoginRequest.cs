using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models.DTO.AccountDTO;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string email { get; set; }
    [Required]
    [MinLength(1)]
    public string password { get; set; }
}