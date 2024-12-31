using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.DTO.GroupDTO;

public class CreateGroupRequest
{
    [Required]
    [MinLength(1)]
    public string name { get; set; }
}