using Lab5.BLL.Exceptions;
using Lab5.BLL.Services;
using Lab5.DAL;
using Lab5.Tests.Doubles;

namespace Lab5.Tests;

public class OtherServicesTests
{
    [Fact]
    public void TaxiDriverService_CompleteTrip_IncrementsTripsAndSaves()
    {

        MockRepository<TaxiDriverEntity> repository = new MockRepository<TaxiDriverEntity>(TestData.NewTaxiEntity("ABC123456", trips: 2));
        TaxiDriverService service = new TaxiDriverService(repository);

     
        service.CompleteTrip("ABC123456");

      
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(3, repository.LastSaved[0].Trips);
    }

    [Fact]
    public void TaxiDriverService_CompleteTripForUnknownDriver_ThrowsNotFound()
    {
       
        TaxiDriverService service = new TaxiDriverService(new StubRepository<TaxiDriverEntity>());
        Action act = () => service.CompleteTrip("ABC123456");

      
        Assert.Throws<EntityNotFoundException>(act);
    }

    [Fact]
    public void AcrobatService_AddExperienceYear_IncrementsExperienceAndSaves()
    {
      
        MockRepository<AcrobatEntity> repository = new MockRepository<AcrobatEntity>(TestData.NewAcrobatEntity("AC12345", 4));
        AcrobatService service = new AcrobatService(repository);

    
        service.AddExperienceYear("AC12345");

    
        Assert.Equal(1, repository.SaveAllCallCount);
        Assert.Equal(5, repository.LastSaved[0].ExperienceYears);
    }

    [Fact]
    public void AcrobatService_AddExperienceYearAtLimit_ThrowsAndDoesNotSave()
    {
      
        MockRepository<AcrobatEntity> repository = new MockRepository<AcrobatEntity>(TestData.NewAcrobatEntity("AC12345", 60));
        AcrobatService service = new AcrobatService(repository);
        Action act = () => service.AddExperienceYear("AC12345");

    
        Assert.Throws<InvalidEntityDataException>(act);

      
        Assert.Equal(0, repository.SaveAllCallCount);
    }
}
