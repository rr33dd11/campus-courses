using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models.DTO.AccountDTO;

public class EditProfileRequest
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    [Required]
    public DateTime birthDate { get; set; }
}