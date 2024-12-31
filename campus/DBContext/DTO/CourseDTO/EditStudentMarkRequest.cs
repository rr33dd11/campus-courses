using System.ComponentModel.DataAnnotations;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class EditStudentMarkRequest
{
    [Required]
    public MarkType markType { get; set; }
    [Required]
    public StudentMarks mark { get; set; }
}