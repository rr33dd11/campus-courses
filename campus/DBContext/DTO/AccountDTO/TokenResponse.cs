using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models.DTO.AccountDTO;

public class TokenResponse
{
    [Required]
    [MinLength(1)]
    public string token { get; set; }


    public TokenResponse(string token)
    {
        this.token = token;
    }
}