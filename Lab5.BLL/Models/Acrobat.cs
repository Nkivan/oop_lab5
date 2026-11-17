using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.BLL.Models;

public class Acrobat : Person
{
    public const int MaxExperienceYears = 60;

    public string CertificateId { get; }
    public int ExperienceYears { get; private set; }

    public override string Key => CertificateId;

    public Acrobat(string lastName, string firstName, string certificateId, int experienceYears,
                   int dances = 0, string driverLicense = null)
        : base(lastName, firstName, dances, driverLicense)
    {
        Validator.Require(Validator.CertificatePattern, certificateId, "Сертифікат акробата");
        Validator.RequireRange(experienceYears, 0, MaxExperienceYears, "Стаж");

        CertificateId = certificateId;
        ExperienceYears = experienceYears;
    }

    public void AddExperienceYear()
    {
        if (ExperienceYears >= MaxExperienceYears)
        {
            throw new InvalidEntityDataException($"Стаж не може перевищувати {MaxExperienceYears} років.");
        }
        ExperienceYears++;
    }
}
