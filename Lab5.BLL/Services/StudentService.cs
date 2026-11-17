using Lab5.BLL.Mapping;
using Lab5.BLL.Models;
using Lab5.BLL.Validation;
using Lab5.DAL;

namespace Lab5.BLL.Services;

public class StudentService : PersonService<Student, StudentEntity>
{
    public StudentService(IRepository<StudentEntity> repository)
        : base(repository, new StudentMapper())
    {
    }


    public FirstYearReport GetFirstYearFromOtherCities(string universityCity)
    {
        Validator.Require(Validator.CityPattern, universityCity, "Місто університету");

        List<Student> firstYear = new List<Student>();
        List<Student> fromOtherCities = new List<Student>();
        foreach (Student student in LoadModels())
        {
            if (student.Course != 1)
            {
                continue;
            }

            firstYear.Add(student);
            if (!string.Equals(student.ArrivalCity, universityCity, StringComparison.OrdinalIgnoreCase))
            {
                fromOtherCities.Add(student);
            }
        }

        double percent = 0;
        if (firstYear.Count > 0)
        {
            percent = fromOtherCities.Count * 100.0 / firstYear.Count;
        }

        return new FirstYearReport(universityCity, firstYear.Count, fromOtherCities.Count, percent, fromOtherCities);
    }

    public void MoveIntoDormitory(string key, string universityCity, int dormitoryNumber, int roomNumber)
    {
        Modify(key, student => student.MoveIntoDormitory(universityCity, dormitoryNumber, roomNumber));
    }
}
