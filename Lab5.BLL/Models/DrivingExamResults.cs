namespace Lab5.BLL.Models;

public class DrivingExamResults
{

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
