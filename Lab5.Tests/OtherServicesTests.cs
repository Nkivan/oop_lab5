using Lab5.BLL.Exceptions;
using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests;

/// <summary>Сервіси додаткових сутностей: перевіряються лише їхні специфічні операції.</summary>
public class OtherServicesTests
{
    [Fact]
    public void TaxiDriverService_CompleteTrip_IncrementsTripsAndSaves()
    {
        // Arrange
        MockRepository<TaxiDriverEntity> repository = new MockRepository<TaxiDriverEntity>(TestData.NewTaxiEntity("ABC123456", trips: 2));
        TaxiDriverService service = new TaxiDriverService(repository);

        // Act
        service.CompleteTrip("ABC123456");

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(3, repository.LastSaved[0].Trips);
    }

    [Fact]
    public void TaxiDriverService_CompleteTripForUnknownDriver_ThrowsNotFound()
    {
        // Arrange
        TaxiDriverService service = new TaxiDriverService(new StubRepository<TaxiDriverEntity>());
        Action act = () => service.CompleteTrip("ABC123456");

        // Act + Assert
        Assert.Throws<EntityNotFoundException>(act);
    }

    [Fact]
    public void AcrobatService_AddExperienceYear_IncrementsExperienceAndSaves()
    {
        // Arrange
        MockRepository<AcrobatEntity> repository = new MockRepository<AcrobatEntity>(TestData.NewAcrobatEntity("AC12345", 4));
        AcrobatService service = new AcrobatService(repository);

        // Act
        service.AddExperienceYear("AC12345");

        // Assert
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(5, repository.LastSaved[0].ExperienceYears);
    }

    [Fact]
    public void AcrobatService_AddExperienceYearAtLimit_ThrowsAndDoesNotSave()
    {
        // Arrange
        MockRepository<AcrobatEntity> repository = new MockRepository<AcrobatEntity>(TestData.NewAcrobatEntity("AC12345", 60));
        AcrobatService service = new AcrobatService(repository);
        Action act = () => service.AddExperienceYear("AC12345");

        // Act
        Assert.Throws<InvalidEntityDataException>(act);

        // Assert
        Assert.Equal(0, repository.SaveAllCallCount);
    }
}
