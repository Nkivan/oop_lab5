using Lab5.BLL.Models;
using Lab5.BLL.Services;
using Lab5.BLL.Validation;
using Lab5.DAL;

namespace Lab5.PL.Menus;

internal sealed class TaxiDriverMenu : EntityMenu<TaxiDriver, TaxiDriverEntity>
{
    private readonly TaxiDriverService _taxiService;

    public TaxiDriverMenu(TaxiDriverService service) : base(service)
    {
        _taxiService = service;
    }

    protected override string Title => "ТАКСИСТИ";

    protected override TaxiDriver ReadModel() => new TaxiDriver(
        ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        ConsoleIO.ReadValid("Посвідчення водія", Validator.DriverLicensePattern, "3 великі латинські літери + 6 цифр, напр. ABC123456"));

    protected override string Describe(TaxiDriver d)
        => $"[Таксист] {d.LastName} {d.FirstName}, посвідчення {d.DriverLicense}, поїздок: {d.Trips}, танців: {d.Dances}";

    protected override TaxiDriver[] Samples() => new[]
    {
        new TaxiDriver("Сидоренко", "Петро", "ABC123456"),
        new TaxiDriver("Гнатюк", "Василь", "ABC654321"),
        new TaxiDriver("Руденко", "Дмитро", "ABC111222")
    };

    protected override MenuItem[] ExtraItems() => new[]
    {
        new MenuItem("Завершити поїздку", CompleteTrip)
    };

    private void CompleteTrip()
    {
        string key = ConsoleIO.ReadLine("Посвідчення водія: ");
        _taxiService.CompleteTrip(key);
        Console.WriteLine(Describe(_taxiService.GetByKey(key)));
    }
}
