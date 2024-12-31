using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.DTO.CourseDTO;

public class EditAnnotationsAndRequirementsRequest
{
    [Required]
    [MinLength(1)]
    public string requirements { get; set; }
    [Required]
    [MinLength(1)]
    public string annotations { get; set; }
}