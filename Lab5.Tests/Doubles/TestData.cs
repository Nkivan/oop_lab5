using Lab5.DAL;

namespace Lab5.Tests.Doubles;

internal static class TestData
{
    public static StudentEntity NewStudentEntity(
        string studentId = "KB123456", string lastName = "Коваленко", int course = 1,
        string city = "Львів", string dormitoryRoom = null, int dances = 0)
        => new StudentEntity
        {
            LastName = lastName, FirstName = "Олена", Course = course, StudentId = studentId,
            ArrivalCity = city, Passport = "AB123456", DormitoryRoom = dormitoryRoom, Dances = dances
        };

    public static TaxiDriverEntity NewTaxiEntity(string license = "ABC123456", int trips = 0)
        => new TaxiDriverEntity { LastName = "Сидоренко", FirstName = "Петро", DriverLicense = license, Trips = trips };

    public static AcrobatEntity NewAcrobatEntity(string certificate = "AC12345", int years = 4)
        => new AcrobatEntity { LastName = "Лисенко", FirstName = "Оксана", CertificateId = certificate, ExperienceYears = years };

    public static StudentEntity[] MixedStudents() => new[]
    {
        NewStudentEntity("KB123456", "Коваленко", 1, "Львів"),
        NewStudentEntity("KB123457", "Мельник", 1, "Київ"),
        NewStudentEntity("KB123458", "Шевченко", 1, "Харків"),
        NewStudentEntity("KB123459", "Бондаренко", 1, "Одеса"),
        NewStudentEntity("KB223460", "Ткаченко", 2, "Львів"),
        NewStudentEntity("KB323461", "Кравченко", 3, "Дніпро")
    };
}
