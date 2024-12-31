namespace campus.DBContext.Models.DTO.AccountDTO;

public class GetProfileResponse
{
    public Guid id { get; set; }
    public string fullName { get; set; }
    public string email { get; set; }
    public DateTime birthDate { get; set; }
}