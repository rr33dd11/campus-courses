using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.DTO.GroupDTO;

public class EditGroupRequest
{
    [Required]
    [MinLength(1)]
    public string name { get; set; }
}