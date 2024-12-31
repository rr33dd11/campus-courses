using System.Text.RegularExpressions;

namespace campus.AdditionalServices.Validators;

public class PasswordValidator
{
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        const string passwordRegex = @"^(?=.*\d).{6,}$";
        return Regex.IsMatch(password, passwordRegex);
    }
}