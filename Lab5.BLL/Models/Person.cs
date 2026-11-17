using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.BLL.Models;


public abstract class Person
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Dances { get; private set; }


    public string DriverLicense { get; private set; }


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


    public void Dance()
    {
        Dances++;
    }


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
