using Lab5.BLL.Models;

namespace Lab5.BLL.Services;

public class FirstYearReport
{
    public string UniversityCity { get; }
    public int FirstYearCount { get; }
    public int FromOtherCitiesCount { get; }
    public double Percent { get; }
    public IReadOnlyList<Student> Students { get; }

    public FirstYearReport(string universityCity, int firstYearCount, int fromOtherCitiesCount,
                           double percent, IReadOnlyList<Student> students)
    {
        UniversityCity = universityCity;
        FirstYearCount = firstYearCount;
        FromOtherCitiesCount = fromOtherCitiesCount;
        Percent = percent;
        Students = students;
    }
}
