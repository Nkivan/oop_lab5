using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;

/// <summary>Вміння «отримання водійського посвідчення при необхідних умовах» (перевіряється на Student).</summary>
public class DriverLicenseTests
{
    private static Student CreateStudent(string driverLicense = null)
        => new Student("Коваленко", "Олена", 1, "KB123456", "Львів", "AB123456", 0, driverLicense);

    private static DrivingExamResults AllConditionsMet(int age = 20)
        => new DrivingExamResults(age, true, true, true);

    [Fact]
    public void ObtainDriverLicense_AllConditionsMet_SetsLicense()
    {
        // Arrange
        Student student = CreateStudent();

        // Act
        student.ObtainDriverLicense(AllConditionsMet(), "ABC123456");

        // Assert
        Assert.Equal("ABC123456", student.DriverLicense);
    }

    [Fact]
    public void ObtainDriverLicense_ExactlyMinimumAge_SetsLicense()
    {
        // Arrange
        Student student = CreateStudent();

        // Act
        student.ObtainDriverLicense(AllConditionsMet(DrivingExamResults.MinimumAge), "ABC123456");

        // Assert
        Assert.Equal("ABC123456", student.DriverLicense);
    }

    [Theory]
    [InlineData(17, true, true, true, "вік")]
    [InlineData(18, false, true, true, "медична")]
    [InlineData(18, true, false, true, "теоретичний")]
    [InlineData(18, true, true, false, "практичний")]
    public void ObtainDriverLicense_ConditionNotMet_ThrowsAndNoLicenseIssued(
        int age, bool medical, bool theory, bool practice, string expectedFragment)
    {
        // Arrange
        Student student = CreateStudent();
        DrivingExamResults results = new DrivingExamResults(age, medical, theory, practice);
        Action act = () => student.ObtainDriverLicense(results, "ABC123456");

        // Act
        DriverLicenseException exception = Assert.Throws<DriverLicenseException>(act);

        // Assert
        Assert.Contains(expectedFragment, exception.Message);
        Assert.Null(student.DriverLicense);
    }

    [Fact]
    public void ObtainDriverLicense_NullResults_ThrowsArgumentNullException()
    {
        // Arrange
        Student student = CreateStudent();
        Action act = () => student.ObtainDriverLicense(null, "ABC123456");

        // Act + Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void ObtainDriverLicense_LicenseAlreadyIssued_ThrowsAndKeepsOldNumber()
    {
        // Arrange
        Student student = CreateStudent(driverLicense: "AAA111111");
        Action act = () => student.ObtainDriverLicense(AllConditionsMet(), "BBB222222");

        // Act
        DriverLicenseException exception = Assert.Throws<DriverLicenseException>(act);

        // Assert
        Assert.Contains("вже отримано", exception.Message);
        Assert.Equal("AAA111111", student.DriverLicense);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ABC12345")]
    [InlineData("abc123456")]
    public void ObtainDriverLicense_InvalidNumber_ThrowsAndNoLicenseIssued(string number)
    {
        // Arrange
        Student student = CreateStudent();
        Action act = () => student.ObtainDriverLicense(AllConditionsMet(), number);

        // Act
        Assert.Throws<InvalidEntityDataException>(act);

        // Assert
        Assert.Null(student.DriverLicense);
    }
}
