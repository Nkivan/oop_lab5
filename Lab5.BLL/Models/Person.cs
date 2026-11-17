using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.BLL.Models;

/// <summary>
/// Базова модель особи (рівень BLL). Усі дані перевіряються тут, у конструкторі.
/// Додаткові вміння варіанта 2: танцювати та отримання водійського посвідчення при необхідних умовах.
/// </summary>
public abstract class Person
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Dances { get; private set; }

    /// <summary>Номер водійського посвідчення (null, якщо посвідчення ще немає).</summary>
    public string DriverLicense { get; private set; }

    /// <summary>Унікальний ключ конкретного типу особи.</summary>
    public abstract string Key { get; }

    protected Person(string lastName, string firstName, int dances, string driverLicense)
    {
        Validator.Require(Validator.NamePattern, lastName, "Прізвище");
        Validator.Require(Validator.NamePattern, firstName, "Ім'я");
        Validator.RequireRange(dances, 0, int.MaxValue, "Кількість танців");
        if (driverLicense != null)
        {
            Validator.Require(Validator.DriverLicensePattern, driverLicense, "Посвідчення водія");
        }

        LastName = lastName;
        FirstName = firstName;
        Dances = dances;
        DriverLicense = driverLicense;
    }

    /// <summary>Вміння «танцювати»: змінює стан об'єкта.</summary>
    public void Dance()
    {
        Dances++;
    }

    /// <summary>
    /// Отримання водійського посвідчення. Умови: посвідчення ще немає, вік не менший за 18,
    /// є медична довідка, складені теоретичний і практичний іспити, номер має коректний формат.
    /// </summary>
    public void ObtainDriverLicense(DrivingExamResults results, string licenseNumber)
    {
        if (results == null)
        {
            throw new ArgumentNullException(nameof(results));
        }
        if (DriverLicense != null)
        {
            throw new DriverLicenseException("Водійське посвідчення вже отримано.");
        }
        if (results.Age < DrivingExamResults.MinimumAge)
        {
            throw new DriverLicenseException($"Мінімальний вік для отримання посвідчення - {DrivingExamResults.MinimumAge} років.");
        }
        if (!results.MedicalCertificate)
        {
            throw new DriverLicenseException("Потрібна медична довідка.");
        }
        if (!results.TheoryExamPassed)
        {
            throw new DriverLicenseException("Не складено теоретичний іспит.");
        }
        if (!results.PracticeExamPassed)
        {
            throw new DriverLicenseException("Не складено практичний іспит.");
        }

        Validator.Require(Validator.DriverLicensePattern, licenseNumber, "Номер посвідчення");
        DriverLicense = licenseNumber;
    }
}
