using System.Text.RegularExpressions;
using Lab5.BLL.Exceptions;

namespace Lab5.BLL.Validation;


public static class Validator
{
    public const string NamePattern = @"^[A-Za-zА-Яа-яІіЇїЄєҐґ'’\-]{2,30}$";
    public const string CityPattern = @"^[A-Za-zА-Яа-яІіЇїЄєҐґ'’\- ]{2,40}$";
    public const string CoursePattern = @"^[1-6]$";
    public const string StudentIdPattern = @"^[A-Z]{2}\d{6}$";
    public const string PassportPattern = @"^[A-ZА-ЯІЇЄҐ]{2}\d{6}$";
    public const string DriverLicensePattern = @"^[A-Z]{3}\d{6}$";
    public const string CertificatePattern = @"^AC\d{5}$";
    public const string DormitoryRoomPattern = @"^[1-9]\d?\.[1-9]\d{0,2}$";

    public static bool IsValid(string pattern, string value)
    {
        if (value == null)
        {
            return false;
        }
        return Regex.IsMatch(value, pattern);
    }

    public static void Require(string pattern, string value, string field)
    {
        if (!IsValid(pattern, value))
        {
            throw new InvalidEntityDataException($"Недопустиме значення '{value}' для поля '{field}'.");
        }
    }

    public static void RequireRange(int value, int min, int max, string field)
    {
        if (value < min || value > max)
        {
            throw new InvalidEntityDataException($"Значення {value} поза допустимим діапазоном [{min}; {max}] для поля '{field}'.");
        }
    }
}
