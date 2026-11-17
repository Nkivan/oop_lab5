using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;

/// <summary>Тести специфічних операцій таксиста (те, що не перевірялося на Student).</summary>
public class TaxiDriverTests
{
    [Fact]
    public void Constructor_ValidData_SetsPropertiesAndKeyIsLicense()
    {
        // Arrange + Act
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456", 3, 2);

        // Assert
        Assert.Equal("ABC123456", driver.DriverLicense);
        Assert.Equal("ABC123456", driver.Key);
        Assert.Equal(3, driver.Trips);
        Assert.Equal(2, driver.Dances);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc123456")]
    public void Constructor_MissingOrInvalidLicense_Throws(string license)
    {
        // Arrange
        Action act = () => new TaxiDriver("Сидоренко", "Петро", license);

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void Constructor_NegativeTrips_Throws()
    {
        // Arrange
        Action act = () => new TaxiDriver("Сидоренко", "Петро", "ABC123456", -1);

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void CompleteTrip_CalledTwice_IncrementsTrips()
    {
        // Arrange
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456");

        // Act
        driver.CompleteTrip();
        driver.CompleteTrip();

        // Assert
        Assert.Equal(2, driver.Trips);
    }

    [Fact]
    public void ObtainDriverLicense_DriverAlreadyHasLicense_Throws()
    {
        // Arrange
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456");
        DrivingExamResults results = new DrivingExamResults(30, true, true, true);
        Action act = () => driver.ObtainDriverLicense(results, "XYZ987654");

        // Act + Assert
        Assert.Throws<DriverLicenseException>(act);
    }
}
