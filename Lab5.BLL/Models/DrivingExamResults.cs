namespace Lab5.BLL.Models;

/// <summary>Результати, за якими вирішується питання про видачу водійського посвідчення.</summary>
public class DrivingExamResults
{
    /// <summary>Мінімальний вік для отримання посвідчення.</summary>
    public const int MinimumAge = 18;

    public int Age { get; }
    public bool MedicalCertificate { get; }
    public bool TheoryExamPassed { get; }
    public bool PracticeExamPassed { get; }

    public DrivingExamResults(int age, bool medicalCertificate, bool theoryExamPassed, bool practiceExamPassed)
    {
        Age = age;
        MedicalCertificate = medicalCertificate;
        TheoryExamPassed = theoryExamPassed;
        PracticeExamPassed = practiceExamPassed;
    }
}
