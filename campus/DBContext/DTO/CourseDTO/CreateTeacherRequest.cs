using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.DTO.CourseDTO;

public class CreateTeacherRequest
{
    [Required]
    public Guid userId { get; set; }
}