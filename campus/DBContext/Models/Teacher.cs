namespace campus.DBContext.Models;

public class Teacher
{
    public Guid AccountId { get; set; }
    public Guid CourseId { get; set; }
    public Account Account { get; set; }
    public Course Course { get; set; }
    public bool IsMain { get; set; }
}