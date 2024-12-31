using campus.DBContext.Models.Enums;

namespace campus.AdditionalServices.Validators;

public static class CourseDateValidator
{
    public static bool isValidCourseStartYear(int startYear, Semesters semester)
    {
        //можно создавать курс в будущих или идущем семестре
        var today = DateTime.Today;
        if (startYear < today.Year)
        {
            return today.Month == 1 && semester == Semesters.Autumn;
        }
        if (startYear == today.Year)
        {
            return !(semester == Semesters.Spring && today.Month > 6);
        }
        return true;
    }
}