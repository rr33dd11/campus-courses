using System.ComponentModel.DataAnnotations;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.Models;

public class Course
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int StartYear { get; set; }
    public int MaximumStudentsCount { get; set; }
    public int RemainingSlotsCount { get; set; }
    public Semesters Semester { get; set; }
    public CourseStatuses Status { get; set; }
    public string Requirements { get; set; }
    public string Annotations { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public DateTime CreatedDate { get; set; } 
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}