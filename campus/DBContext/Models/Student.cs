using campus.DBContext.Models.Enums;

namespace campus.DBContext.Models;

public class Student
{
    public Guid AccountId { get; set; }
    public Guid CourseId { get; set; }
    public Account Account { get; set; }
    public Course Course { get; set; }
    public StudentMarks MidtermResult { get; set; }
    public StudentMarks FinalResult { get; set; }
    public StudentStatuses Status { get; set; }
}