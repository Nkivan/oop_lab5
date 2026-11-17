using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;

public class TaxiDriverTests
{
    [Fact]
    public void Constructor_ValidData_SetsPropertiesAndKeyIsLicense()
    {
     
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456", 3, 2);

      
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

        Action act = () => new TaxiDriver("Сидоренко", "Петро", license);

    
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void Constructor_NegativeTrips_Throws()
    {
    
        Action act = () => new TaxiDriver("Сидоренко", "Петро", "ABC123456", -1);

     
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void CompleteTrip_CalledTwice_IncrementsTrips()
    {
     
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456");

     
        driver.CompleteTrip();
        driver.CompleteTrip();

     
        Assert.Equal(2, driver.Trips);
    }

    [Fact]
    public void ObtainDriverLicense_DriverAlreadyHasLicense_Throws()
    {
      
        TaxiDriver driver = new TaxiDriver("Сидоренко", "Петро", "ABC123456");
        DrivingExamResults results = new DrivingExamResults(30, true, true, true);
        Action act = () => driver.ObtainDriverLicense(results, "XYZ987654");

      
        Assert.Throws<DriverLicenseException>(act);
    }
}
