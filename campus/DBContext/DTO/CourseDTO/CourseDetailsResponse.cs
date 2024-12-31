using campus.DBContext.Models;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class CourseDetailsResponse
{
    public Guid id { get; set; }
    public string name { get; set; }
    public int startYear { get; set; }
    public int maximumStudentsCount { get; set; }
    public int studentsEnrolledCount { get; set; }
    public int studentsInQueueCount { get; set; }
    public CourseStatuses? status { get; set; }
    public Semesters semester { get; set; }
    public string requirements { get; set; }
    public string annotaitons { get; set; }
    public List<StudentDTO> students { get; set; }
    public List<TeacherDTO> teachers { get; set; }
    public List<NotificationDTO> notifications { get; set; }
}