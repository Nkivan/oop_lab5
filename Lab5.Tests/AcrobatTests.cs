using Lab5.BLL.Exceptions;
using Lab5.BLL.Models;

namespace Lab5.Tests;


public class AcrobatTests
{
    [Fact]
    public void Constructor_ValidData_SetsPropertiesAndKeyIsCertificate()
    {
     
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

      
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
        
        Action act = () => new Acrobat("Лисенко", "Оксана", certificate, 4);

       
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(61)]
    public void Constructor_ExperienceOutOfRange_Throws(int years)
    {
        
        Action act = () => new Acrobat("Лисенко", "Оксана", "AC12345", years);

     
        Assert.Throws<InvalidEntityDataException>(act);
    }

    [Fact]
    public void AddExperienceYear_BelowLimit_IncrementsExperience()
    {
      
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

      
        acrobat.AddExperienceYear();

       
        Assert.Equal(5, acrobat.ExperienceYears);
    }

    [Fact]
    public void AddExperienceYear_AtLimit_ThrowsAndExperienceUnchanged()
    {
       
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", Acrobat.MaxExperienceYears);
        Action act = () => acrobat.AddExperienceYear();

      
        Assert.Throws<InvalidEntityDataException>(act);

      
        Assert.Equal(Acrobat.MaxExperienceYears, acrobat.ExperienceYears);
    }

    [Fact]
    public void Dance_Once_IncrementsDances()
    {
      
        Acrobat acrobat = new Acrobat("Лисенко", "Оксана", "AC12345", 4);

       
        acrobat.Dance();

     
        Assert.Equal(1, acrobat.Dances);
    }
}
