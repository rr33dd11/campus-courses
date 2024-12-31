using System.ComponentModel.DataAnnotations;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class EditStudentStatusRequest
{
    [Required]
    public StudentStatuses status { get; set; }
}