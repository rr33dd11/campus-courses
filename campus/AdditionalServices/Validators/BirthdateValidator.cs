namespace campus.AdditionalServices.Validators;

public static class BirthdateValidator
{
    public static bool IsValidBirthday(DateTime birthday)
    {
        DateTime minDate = DateTime.Today.AddYears(-99); // Самый старший возраст — 99 лет назад
        DateTime maxDate = DateTime.Today.AddYears(-14); // Самый младший возраст — 14 лет назад

        return birthday >= minDate && birthday <= maxDate;
    }
}