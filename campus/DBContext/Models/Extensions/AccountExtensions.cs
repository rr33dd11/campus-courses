using campus.DBContext.DTO.UsersDTO;
using campus.DBContext.Models;
using campus.DBContext.Models.DTO.AccountDTO;

namespace campus.DBContext.Extensions;

public static class AccountExtensions
{
    public static UserResponse ToUserResponse(this Account account)
    {
        return new UserResponse()
        {
            id = account.Id,
            fullName = account.FullName,
        };
    }

    public static RolesResponse ToRolesResponse(this Account account)
    {
        return new RolesResponse()
        {
            isAdmin = account.isAdmin,
            isStudent = account.isStudent,
            isTeacher = account.isTeacher
        };
    }

    public static GetProfileResponse ToGetProfileResponse(this Account account)
    {
        return new GetProfileResponse()
        {
            id = account.Id,
            fullName = account.FullName,
            birthDate = account.BirthDate,
            email = account.Email
        };   
    }
}