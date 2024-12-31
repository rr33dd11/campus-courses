using campus.DBContext.Models.Enums;

namespace campus.DBContext.DTO.CourseDTO;

public class CourseResponse
{
    public Guid id { get; set; }
    public string name { get; set; }
    public int startYear { get; set; }
    public int mmaximusStudentsCount { get; set; }
    public int remainingSlotsCount { get; set; }
    public CourseStatuses status { get; set; }
    public Semesters semester { get; set; }
}