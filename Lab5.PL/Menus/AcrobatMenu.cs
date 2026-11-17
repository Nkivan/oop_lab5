using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.BLL.Validation;
using Lab5.DAL;

namespace Lab5.PL.Menus;

internal sealed class AcrobatMenu : EntityMenu<Acrobat, AcrobatEntity>
{
    private readonly AcrobatService _acrobatService;

    public AcrobatMenu(AcrobatService service) : base(service)
    {
        _acrobatService = service;
    }

    protected override string Title => "АКРОБАТИ";

    protected override Acrobat ReadModel() => new Acrobat(
        ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        ConsoleIO.ReadValid("Сертифікат акробата", Validator.CertificatePattern, "AC + 5 цифр, напр. AC12345"),
        ConsoleIO.ReadInt("Стаж (років, 0-60): "));

    protected override string Describe(Acrobat a)
        => $"[Акробат] {a.LastName} {a.FirstName}, сертифікат {a.CertificateId}, стаж {a.ExperienceYears} р., " +
           $"посвідчення: {(a.DriverLicense ?? "немає")}, танців: {a.Dances}";

    protected override Acrobat[] Samples() => new[]
    {
        new Acrobat("Лисенко", "Оксана", "AC12345", 4),
        new Acrobat("Мороз", "Тарас", "AC54321", 7),
        new Acrobat("Савчук", "Ірина", "AC11111", 2)
    };

    protected override MenuItem[] ExtraItems() => new[]
    {
        new MenuItem("Додати рік стажу", AddExperienceYear)
    };

    private void AddExperienceYear()
    {
        string key = ConsoleIO.ReadLine("Сертифікат: ");
        _acrobatService.AddExperienceYear(key);
        Console.WriteLine(Describe(_acrobatService.GetByKey(key)));
    }
}
