using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.BLL.Validation;
using Lab5.DAL;

namespace Lab5.PL.Menus;

internal sealed class StudentMenu : EntityMenu<Student, StudentEntity>
{
    private readonly StudentService _studentService;

    public StudentMenu(StudentService service) : base(service)
    {
        _studentService = service;
    }

    protected override string Title => "СТУДЕНТИ";

    protected override Student ReadModel() => new Student(
        ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        int.Parse(ConsoleIO.ReadValid("Курс", Validator.CoursePattern, "число від 1 до 6")),
        ConsoleIO.ReadValid("Студентський квиток", Validator.StudentIdPattern, "2 великі латинські літери + 6 цифр, напр. KB123456"),
        ConsoleIO.ReadValid("Місто прибуття", Validator.CityPattern, "літери, 2-40 символів"),
        ConsoleIO.ReadValid("Паспорт (серія і номер)", Validator.PassportPattern, "2 великі літери + 6 цифр, напр. AB123456"));

    protected override string Describe(Student s)
        => $"[Студент] {s.LastName} {s.FirstName}, курс {s.Course}, квиток {s.StudentId}, місто прибуття: {s.ArrivalCity}, " +
           $"паспорт {s.Passport}, гуртожиток: {(s.LivesInDormitory ? s.DormitoryRoom : "ні")}, " +
           $"посвідчення: {(s.DriverLicense ?? "немає")}, танців: {s.Dances}";

    protected override Student[] Samples() => new[]
    {
        new Student("Коваленко", "Олена", 1, "KB123456", "Львів", "AB123456"),
        new Student("Мельник", "Андрій", 1, "KB123457", "Київ", "AB123457"),
        new Student("Шевченко", "Марія", 1, "KB123458", "Харків", "AB123458"),
        new Student("Бондаренко", "Ігор", 1, "KB123459", "Одеса", "AB123459"),
        new Student("Ткаченко", "Софія", 2, "KB223460", "Київ", "AB123460"),
        new Student("Кравченко", "Максим", 3, "KB323461", "Дніпро", "AB123461")
    };

    protected override MenuItem[] ExtraItems() => new[]
    {
        new MenuItem("Оселити в гуртожиток (лише для приїжджих)", MoveIntoDormitory),
        new MenuItem("Завдання варіанта: відсоток студентів 1-го курсу з інших міст", RunVariantTask)
    };

    private void MoveIntoDormitory()
    {
        string key = ConsoleIO.ReadLine("Студентський квиток: ");
        string city = ReadUniversityCity();
        int dormitory = ConsoleIO.ReadInt("Номер гуртожитку: ");
        int room = ConsoleIO.ReadInt("Номер кімнати: ");

        _studentService.MoveIntoDormitory(key, city, dormitory, room);
        Console.WriteLine("Поселено. Новий стан:");
        Console.WriteLine(Describe(_studentService.GetByKey(key)));
    }

    private void RunVariantTask()
    {
        FirstYearReport report = _studentService.GetFirstYearFromOtherCities(ReadUniversityCity());

        Console.WriteLine($"Студентів 1-го курсу: {report.FirstYearCount}");
        Console.WriteLine($"З інших міст (не з міста {report.UniversityCity}): {report.FromOtherCitiesCount}");
        Console.WriteLine($"Відсоток: {report.Percent:F1}%");
        foreach (Student student in report.Students)
        {
            Console.WriteLine("  " + Describe(student));
        }
    }

    private static string ReadUniversityCity()
    {
        string city = ConsoleIO.ReadLine("Місто університету (Enter = Київ): ");
        return city.Length == 0 ? "Київ" : city;
    }
}
