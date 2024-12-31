using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class StudentDTO
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public StudentStatuses status { get; set; }
    public StudentMarks midtermResult { get; set; }
    public StudentMarks finalResult { get; set; }
}