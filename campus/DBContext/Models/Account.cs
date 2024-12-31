using System.ComponentModel.DataAnnotations;

namespace campus.DBContext.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public bool isTeacher { get; set; }
        public bool isAdmin { get; set; }
        public bool isStudent { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public ICollection<Student> MyCourses { get; set; } = new List<Student>();
        public ICollection<Teacher> TeachingCourses { get; set; } = new List<Teacher>();
        
        
    }
}