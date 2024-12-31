using campus.DBContext.DTO.CourseDTO;
using campus.DBContext.Models;
using campus.DBContext.Models.Enums;

namespace campus.DBContext.Extensions;

public static class StudentExtensions
{
    public static StudentDTO ToStudentDto(this Student student, bool isStudent, Guid accountId)
    {
        return new StudentDTO()
        {
            id = student.AccountId,
            name = student.Account.FullName,
            email = student.Account.Email,
            status = student.Status,
            midtermResult = (!isStudent || student.AccountId == accountId) ? student.MidtermResult : StudentMarks.NotDefined,
            finalResult = (!isStudent || student.AccountId == accountId) ? student.FinalResult : StudentMarks.NotDefined
        };
    }
}