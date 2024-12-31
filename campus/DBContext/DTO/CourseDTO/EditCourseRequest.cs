using System.ComponentModel.DataAnnotations;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class EditCourseRequest
{
    [Required]
    [MinLength(1)]
    public string name { get; set; }
    [Required]
    [Range(2000, 2029)]
    public int startYear { get; set; }
    [Required]
    [Range(1,200)]
    public int maximumStudentsCount { get; set; }
    [Required]
    public Semesters semester { get; set; }
    [Required]
    [MinLength(1)]
    public string requirements { get; set; }
    [Required]
    [MinLength(1)]
    public string annotaitons { get; set; }
    [Required]
    public Guid mainTeacherId { get; set; }
}