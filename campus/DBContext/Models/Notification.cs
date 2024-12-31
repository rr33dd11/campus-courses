using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models;

public class Notification
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsImportant { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public Course Course { get; set; }
    public Guid CourseId { get; set; }
}