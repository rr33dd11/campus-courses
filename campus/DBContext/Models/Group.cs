using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models;

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public DateTime CreatedDate { get; set; }
}