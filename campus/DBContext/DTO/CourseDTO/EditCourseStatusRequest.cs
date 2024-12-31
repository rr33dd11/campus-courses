using System.ComponentModel.DataAnnotations;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class EditCourseStatusRequest
{
    [Required]
    public CourseStatuses status { get; set; }
}