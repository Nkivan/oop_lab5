using Lab5.BLL.Validation;

namespace Lab5.BLL.Models;

public class TaxiDriver : Person
{
    public int Trips { get; private set; }

    /// <summary>Ключ таксиста - його водійське посвідчення (воно обов'язкове).</summary>
    public override string Key => DriverLicense;

    public TaxiDriver(string lastName, string firstName, string driverLicense, int trips = 0, int dances = 0)
        : base(lastName, firstName, dances, driverLicense)
    {
        Validator.Require(Validator.DriverLicensePattern, driverLicense, "Посвідчення водія");
        Validator.RequireRange(trips, 0, int.MaxValue, "Кількість поїздок");
        Trips = trips;
    }

    /// <summary>Специфічна операція таксиста: завершити поїздку.</summary>
    public void CompleteTrip()
    {
        Trips++;
    }
}
