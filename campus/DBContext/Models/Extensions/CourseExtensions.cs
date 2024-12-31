using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Models;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.Extensions;

public static class CourseExtensions
{
    public static CourseResponse ToCourseResponse(this Course course)
    {
        return new CourseResponse()
        {
            id = course.Id,
            name = course.Name,
            startYear = course.StartYear,
            mmaximusStudentsCount = course.MaximumStudentsCount,
            remainingSlotsCount = course.RemainingSlotsCount,
            status = course.Status,
            semester = course.Semester,
        };
    }

    public static CourseDetailsResponse toCourseDetailsResponse(this Course course, Account account)
    {
        var isTeacherOrAdmin = account.isAdmin || course.Teachers.Any(t => t.AccountId == account.Id);

        var students = course.Students
            .OrderBy(s => s.Status)
            .ThenBy(s => s.Account.FullName)
            .Select(student => student.ToStudentDto(isTeacherOrAdmin, account.Id));
        
        if (!isTeacherOrAdmin)
        {
            students = students.Where(student => student.status == StudentStatuses.Accepted);
        }
        
        var studentsList = students.ToList();

        return new CourseDetailsResponse()
        {
            id = course.Id,
            name = course.Name,
            startYear = course.StartYear,
            maximumStudentsCount = course.MaximumStudentsCount,
            studentsEnrolledCount = course.Students.Count(s => s.Status == StudentStatuses.Accepted),
            studentsInQueueCount = course.Students.Count(s => s.Status == StudentStatuses.InQueue),
            requirements = course.Requirements,
            annotaitons = course.Annotations,
            status = course.Status,
            semester = course.Semester,
            students = studentsList,
            teachers = course.Teachers
                .OrderByDescending(t => t.IsMain)
                .ThenBy(t => t.Account.FullName)
                .Select(t => t.ToTeacherDto())
                .ToList(),
            notifications = course.Notifications
                .OrderBy(c => c.CreatedDate)
                .Select(notification => notification.ToNotificationDto())
                .ToList()
        };
    }
}