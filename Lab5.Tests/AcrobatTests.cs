using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;

/// <summary>Тести специфічних операцій акробата (те, що не перевірялося на Student).</summary>
public class AcrobatTests
{
    [Fact]
    public void Constructor_ValidData_SetsPropertiesAndKeyIsCertificate()
    {
        // Arrange + Act
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

        // Assert
        Assert.Equal("AC12345", acrobat.CertificateId);
        Assert.Equal("AC12345", acrobat.Key);
        Assert.Equal(4, acrobat.ExperienceYears);
        Assert.Null(acrobat.DriverLicense);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("AC1234")]
    [InlineData("ab12345")]
    public void Constructor_InvalidCertificate_Throws(string certificate)
    {
        // Arrange
        Action act = () => new Acrobat("Лисенко", "Оксана", certificate, 4);

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(61)]
    public void Constructor_ExperienceOutOfRange_Throws(int years)
    {
        // Arrange
        Action act = () => new Acrobat("Лисенко", "Оксана", "AC12345", years);

        // Act + Assert
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void AddExperienceYear_BelowLimit_IncrementsExperience()
    {
        // Arrange
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

        // Act
        acrobat.AddExperienceYear();

        // Assert
        Assert.Equal(5, acrobat.ExperienceYears);
    }

    [Fact]
    public void AddExperienceYear_AtLimit_ThrowsAndExperienceUnchanged()
    {
        // Arrange
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", Acrobat.MaxExperienceYears);
        Action act = () => acrobat.AddExperienceYear();

        // Act
        Assert.Throws<InvalidEntityDataException>(act);

        // Assert
        Assert.Equal(Acrobat.MaxExperienceYears, acrobat.ExperienceYears);
    }

    [Fact]
    public void Dance_Once_IncrementsDances()
    {
        // Arrange
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

        // Act
        acrobat.Dance();

        // Assert
        Assert.Equal(1, acrobat.Dances);
    }
}
