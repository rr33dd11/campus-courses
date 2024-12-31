using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Models;

namespace campus.DBContext.Extensions;

public static class TeacherExtensions
{
    public static TeacherDTO ToTeacherDto(this Teacher teacher)
    {
        return new TeacherDTO()
        {
            email = teacher.Account.Email,
            name = teacher.Account.FullName,
            isMain = teacher.IsMain,
        };
    }
}