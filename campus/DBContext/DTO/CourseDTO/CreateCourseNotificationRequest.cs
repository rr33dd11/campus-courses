using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.DTO.CourseDTO;

public class CreateCourseNotificationRequest
{
    [Required]
    [MinLength(1)]
    public string text { get; set; }
    [Required]
    public bool isImportant { get; set; }
}