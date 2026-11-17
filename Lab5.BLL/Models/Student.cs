using Lab5.BLL.Exceptions;
using Lab5.BLL.Validation;

namespace Lab5.BLL.Models;

public class Student : Person
{
    public int Course { get; }
    public string StudentId { get; }
    public string ArrivalCity { get; }
    public string Passport { get; }

    /// <summary>Гуртожиток і кімната у форматі «номер.кімната» (null, якщо студент не поселений).</summary>
    public string DormitoryRoom { get; private set; }

    public bool LivesInDormitory => DormitoryRoom != null;

    public override string Key => StudentId;

    public Student(string lastName, string firstName, int course, string studentId, string arrivalCity,
                   string passport, int dances = 0, string driverLicense = null, string dormitoryRoom = null)
        : base(lastName, firstName, dances, driverLicense)
    {
        Validator.Require(Validator.CoursePattern, course.ToString(), "Курс");
        Validator.Require(Validator.StudentIdPattern, studentId, "Студентський квиток");
        Validator.Require(Validator.CityPattern, arrivalCity, "Місто прибуття");
        Validator.Require(Validator.PassportPattern, passport, "Паспорт");
        if (dormitoryRoom != null)
        {
            Validator.Require(Validator.DormitoryRoomPattern, dormitoryRoom, "Гуртожиток і кімната");
        }

        Course = course;
        StudentId = studentId;
        ArrivalCity = arrivalCity;
        Passport = passport;
        DormitoryRoom = dormitoryRoom;
    }

    /// <summary>
    /// Поселення в гуртожиток: можливе лише якщо студент приїхав з іншого міста (не з міста університету)
    /// і ще не поселений.
    /// </summary>
    public void MoveIntoDormitory(string universityCity, int dormitoryNumber, int roomNumber)
    {
        Validator.Require(Validator.CityPattern, universityCity, "Місто університету");
        if (LivesInDormitory)
        {
            throw new DormitoryException("Студент уже проживає в гуртожитку.");
        }
        if (string.Equals(ArrivalCity, universityCity, StringComparison.OrdinalIgnoreCase))
        {
            throw new DormitoryException("Поселення можливе лише для студентів, які приїхали з іншого міста.");
        }

        string room = $"{dormitoryNumber}.{roomNumber}";
        Validator.Require(Validator.DormitoryRoomPattern, room, "Гуртожиток і кімната");
        DormitoryRoom = room;
    }
}
